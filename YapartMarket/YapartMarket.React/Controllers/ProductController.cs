﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Transactions;
using AutoMapper;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using YapartMarket.Core.BL;
using YapartMarket.Core.Data.Interfaces.Access;
using YapartMarket.Core.Data.Interfaces.Azure;
using YapartMarket.Core.Models;
using YapartMarket.React.ViewModels;

namespace YapartMarket.React.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;

        private readonly List<YapartMarket.Core.Models.Product> _products;
        public ProductController(IMapper mapper, IConfiguration configuration)
        {
            //_accessProductRepository = accessProductRepository;
            _mapper = mapper;
            _configuration = configuration;
            _products = new List<YapartMarket.Core.Models.Product>()
            {
                new YapartMarket.Core.Models.Product()
                {
                    Id = 1, Article = "No-Po-1",
                    Descriptions = "First",
                    Brand = new Brand(){Name = "Norplast", Picture = new Picture(){Path ="\\No-Po-1.png" }},
                    Price = (decimal) 15.6
                },
                new YapartMarket.Core.Models.Product()
                {
                    Id = 2, Article = "NO-1231", Descriptions = "Second",
                    Brand = new Brand(){Name = "Norplast", Picture = new Picture(){Path = "\\NO-1231.png"}},
                    Price = (decimal) 13.2
                }
            };
        }
        

        [HttpGet]
        [Route("products")]
        [Produces("application/json")]
        public IActionResult GetAzureProducts()
        {
            try
            {
                using (var connection = new SqlConnection(_configuration.GetConnectionString("SQLServerConnectionString")))
                {
                    var sql = "select * from dbo.products";
                    connection.Open();
                    var products = connection.Query<Core.Models.Azure.Product>(sql).ToList();
                    return Ok(products);
                }
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost]
        [Route("order/accept")]
        [Produces("application/json")]
        public async Task<IActionResult> AcceptOrder([FromBody] OrderDto orderDto)
        {
            if (orderDto != null)
            {
                var isAccepted = true;
                await using (var connection = new SqlConnection(_configuration.GetConnectionString("SQLServerConnectionString")))
                {
                    //Пройтись по всем товарам, если хоть одного нету или количество меньше того что есть на сервере = отменить заказа
                    connection.Open();
                    foreach (var orderItem in orderDto.OrderInfoDto.OrderItemsDto)
                    {
                        var productInDb = await connection.QueryFirstOrDefaultAsync<Product>("select * from products where sku = @sku and count >= @count", new {sku = orderItem.OfferId, count = orderItem.Count});
                        if (productInDb == null)
                        {
                            isAccepted = false;
                            break;
                        }
                    }
                }
                return Ok(isAccepted ? (object) new OrderViewModel() {OrderInfoViewModel = new OrderInfoViewModel() {Accepted = true, Id = orderDto.OrderInfoDto.Id.ToString()}} : new { accepted = false, id = orderDto.OrderInfoDto.Id.ToString(), reason = "OUT_OF_DATE" });
            }
            return BadRequest();
        }

        [HttpPost]
        [Route("order/status")]
        [Produces("application/json")]
        public IActionResult SetOrderStatus([FromBody] OrderDto orderStatusDto)
        {
            if (orderStatusDto != null)
            {
                return Ok();
            }
            return BadRequest();
        }


        [HttpPost]
        [Route("cart")]
        [Produces("application/json")]
        public async Task<IActionResult> GetInfoFromCart([FromBody] CartDto cartDto, [FromQuery(Name = "auth-token")] string authToken)
        {
            if (string.IsNullOrEmpty(_configuration.GetValue<string>("auth-token")))
                return StatusCode(500);
            if (string.IsNullOrEmpty(authToken) || (!string.IsNullOrEmpty(_configuration.GetValue<string>("auth-token")) && !string.IsNullOrEmpty(authToken) && _configuration.GetValue<string>("auth-token") != authToken))
                return StatusCode(403);
            if (cartDto != null)
            {
                if (cartDto.Cart.CartItems != null)
                {
                    var cartViewModel = new CartViewModel()
                    {
                       Cart = new CartInfoViewModel()
                       {
                           CartItems = new List<CartItemViewModel>()
                       }
                    };
                    await using (var connection = new SqlConnection(_configuration.GetConnectionString("SQLServerConnectionString")))
                    {
                        connection.Open();
                        foreach (var cartItemDto in cartDto.Cart.CartItems)
                        {
                            var isDelivery = false;
                            var count = 0;
                            var productInDb = await connection.QueryFirstOrDefaultAsync<Core.Models.Azure.Product>("select * from products where sku = @sku",
                                new { sku = cartItemDto.OfferId});
                            if (productInDb != null)
                            {
                                isDelivery = true;
                                count = productInDb.Count >= cartItemDto.Count ? cartItemDto.Count : productInDb.Count;
                            }

                            cartViewModel.Cart.CartItems.Add(new CartItemViewModel()
                            {
                                FeedId = cartItemDto.FeedId,
                                OfferId = cartItemDto.OfferId,
                                Count =  count,
                                Delivery = isDelivery
                            });
                        }
                    }
                    //если в заказе все позиции отсуствуют items = пустым.
                    if (cartViewModel.Cart.CartItems.All(x => x.Count == 0))
                        cartViewModel.Cart.CartItems = new List<CartItemViewModel>();
                    return Ok(cartViewModel);
                }
            }
            return BadRequest();
        }

        [HttpPost]
        [Route("setProducts")]
        [Produces("application/json")]
        public async Task<IActionResult> SetProducts([FromBody] ItemsDto itemsDto)
        {
            if (itemsDto != null)
            {
                try
                {
                    using (var connection = new SqlConnection(_configuration.GetConnectionString("SQLServerConnectionString")))
                    {
                        await connection.OpenAsync();
                        await connection.ExecuteAsync(
                            "update products set count = @count, updatedAt = @updatedAt",
                            new
                            {
                                count = 0,
                                updatedAt = DateTimeOffset.Now.ToString("yyyy-MM-dd'T'HH:mm:ssK"),
                            });
                        var productsByInsertSkuInDb = await connection.QueryAsync<Core.Models.Azure.Product>("select * from products where sku IN @skus", new { skus = itemsDto.Products.Select(x => x.Sku) });
                        var updateProducts = itemsDto.Products.Where(x => productsByInsertSkuInDb.Any(t => t.Sku.Equals(x.Sku) && t.Count != x.Count));
                        var insertProducts = itemsDto.Products.Where(x => productsByInsertSkuInDb.All(t => t.Sku != x.Sku));
                        if (updateProducts.Any())
                        {
                            foreach (var updateProduct in updateProducts)
                            {
                                await connection.ExecuteAsync(
                                    "update products set count = @count, updatedAt = @updatedAt where sku = @sku",
                                    new
                                    {
                                        count = updateProduct.Count,
                                        updatedAt = DateTimeOffset.Now.ToString("yyyy-MM-dd'T'HH:mm:ssK"),
                                        sku = updateProduct.Sku
                                    });
                            }
                        }
                        if (insertProducts.Any())
                        {
                            foreach (var insertProduct in insertProducts)
                            {
                                await connection.ExecuteAsync("insert into products(sku, count, updatedAt, type)  values(@sku, @count, @updatedAt, @type)", new
                                {
                                    sku = insertProduct.Sku,
                                    count = insertProduct.Count,
                                    updatedAt = DateTimeOffset.Now.ToString("yyyy-MM-dd'T'HH:mm:ssK"),
                                    type = nameof(ProductType.FIT)
                                });
                            }
                        }

                    }
                }
                catch (Exception e)
                {
                    return BadRequest(e.Message);
                }
                return Ok();
            }
            return BadRequest();
        }

        [HttpPost]
        [Route("stocks")]
        [Produces("application/json")]
        public IActionResult Stocks([FromBody] StockDto stockDto)
        {
            if (stockDto == null)
                return BadRequest();
            var listSkuInfo = new List<SkuInfoDto>();
            using (var connection = new SqlConnection(_configuration.GetConnectionString("SQLServerConnectionString")))
            {
                connection.Open();
                var productsFromDb = connection.Query<Core.Models.Azure.Product>("select * from dbo.products").ToList();
                foreach (var productFromDb in productsFromDb.Where(x=> stockDto.Skus.Any(t=> x.Sku.Equals(t))))
                {
                    listSkuInfo.Add(new SkuInfoDto
                    {
                        Sku = productFromDb.Sku,
                        WarehouseId = stockDto.WarehouseId,
                        Items = new List<ProductDto>
                        {
                            new ProductDto {Type = nameof(ProductType.FIT),
                                Count = productFromDb.Count,
                                UpdatedAt = productFromDb.UpdatedAt}
                        }
                    });
                }
            }
            return Ok(new StocksSkuDto() { Skus = listSkuInfo });
        }

        [HttpGet]
        [Route("ListProducts")]
        public ActionResult ListProducts()
        {
            return Ok(_mapper.Map<List<ProductViewModel>>(_products));
        }

        [HttpGet]
        public ProductViewModel GetProductById(int id)
        {
            return _mapper.Map<ProductViewModel>(_products.FirstOrDefault(x => x.Id == id));
        }
    }

    public class ItemsDto
    {
        public List<ItemDto> Products { get; set; }
    }

    public class ItemDto
    {
        public string Sku { get; set; }
        public int Count { get; set; }
    }

    public class StockDto
    {
        public Int64 WarehouseId { get; set; }
        public List<string> Skus { get; set; }
    }

    public class StocksSkuDto
    {
        [JsonPropertyName("skus")]
        public List<SkuInfoDto> Skus { get; set; }
    }

    public class SkuInfoDto
    {
        [JsonPropertyName("sku")]
        public string Sku { get; set; }
        [JsonPropertyName("warehouseId")]
        public Int64 WarehouseId { get; set; }
        [JsonPropertyName("items")]
        public List<ProductDto> Items { get; set; }
    }

    public class ProductDto
    {
        [JsonPropertyName("type")]
        public string Type { get; set; }
        [JsonPropertyName("count")]
        public Int64 Count { get; set; }
        [JsonPropertyName("updatedAt")]
        public string UpdatedAt { get; set; }
    }

    public enum ProductType
    {
        FIT
    }
}