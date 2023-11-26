using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using YapartMarket.Core.DTO.Yandex;
using YapartMarket.Core.Models.Azure;
using YapartMarket.React.ViewModels;

namespace YapartMarket.React.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ExpressProductController : Controller
    {
        private readonly IConfiguration _configuration;

        public ExpressProductController(IConfiguration configuration)
        {
            _configuration = configuration;
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
                    var sql = "select * from express_products";
                    connection.Open();
                    var products = connection.Query<ExpressProduct>(sql).ToList();
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
                using (var connection = new SqlConnection(_configuration.GetConnectionString("SQLServerConnectionString")))
                {
                    await connection.OpenAsync();
                    foreach (var orderItem in orderDto.OrderInfoDto.OrderItemsDto)
                    {
                        var productInDb = await connection.QueryFirstOrDefaultAsync<ExpressProduct>("select * from express_products where sku = @sku and count >= @count", new { sku = orderItem.OfferId, count = orderItem.Count });
                        if (productInDb == null)
                        {
                            isAccepted = false;
                            break;
                        }
                    }
                }
                return Ok(isAccepted ? new OrderViewModel() { OrderInfoViewModel = new OrderInfoViewModel() { Accepted = true, Id = orderDto.OrderInfoDto.Id.ToString() } } : new { accepted = false, id = orderDto.OrderInfoDto.Id.ToString(), reason = "OUT_OF_DATE" });
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
        public async Task<IActionResult> GetInfoFromCart([FromBody] CartDto cartDto, [FromQuery(Name = "auth-token-express")] string authToken)
        {
            if (string.IsNullOrEmpty(_configuration.GetValue<string>("auth-token-express")))
                return StatusCode(500);
            if (string.IsNullOrEmpty(authToken) || (!string.IsNullOrEmpty(_configuration.GetValue<string>("auth-token-express")) && !string.IsNullOrEmpty(authToken) && _configuration.GetValue<string>("auth-token-express") != authToken))
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
                    using (var connection = new SqlConnection(_configuration.GetConnectionString("SQLServerConnectionString")))
                    {
                        await connection.OpenAsync();
                        foreach (var cartItemDto in cartDto.Cart.CartItems)
                        {
                            var isDelivery = false;
                            var count = 0;
                            var productInDb = await connection.QueryFirstOrDefaultAsync<ExpressProduct>("select * from express_products where sku = @sku",
                                new { sku = cartItemDto.OfferId });
                            if (productInDb != null)
                            {
                                isDelivery = true;
                                count = productInDb.Count >= cartItemDto.Count ? cartItemDto.Count : productInDb.Count;
                            }

                            cartViewModel.Cart.CartItems.Add(new CartItemViewModel()
                            {
                                FeedId = cartItemDto.FeedId,
                                OfferId = cartItemDto.OfferId,
                                Count = count,
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
        public IActionResult SetProducts([FromBody] ItemsDto itemsDto)
        {
            if (itemsDto != null)
            {
                try
                {
                    using (var connection = new SqlConnection(_configuration.GetConnectionString("SQLServerConnectionString")))
                    {
                        connection.Open();
                        var productsInDb = connection.Query<ExpressProduct>("select * from express_products where sku IN @skus",
                            new { skus = itemsDto.Products.Select(x => x.Sku) });
                        var updateProducts = itemsDto.Products.Where(x => productsInDb.Any(t => t.Sku.Equals(x.Sku) && t.Count != x.Count));
                        var insertProducts = itemsDto.Products.Where(x => productsInDb.All(t => t.Sku != x.Sku));
                        if (updateProducts.Any())
                        {
                            foreach (var updateProduct in updateProducts)
                            {
                                connection.Execute(
                                    "update express_products set count = @count, updatedAt = @updatedAt where sku = @sku",
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
                                connection.Execute("insert into express_products(sku, count, created, updatedAt, type)  values(@sku, @count, @created, @updatedAt, @type)", new
                                {
                                    sku = insertProduct.Sku,
                                    created = DateTimeOffset.Now.ToString("yyyy-MM-dd'T'HH:mm:ssK"),
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
                    return BadRequest(e.StackTrace);
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
                var productsFromDb = connection.Query<ExpressProduct>("select * from express_products").ToList();
                foreach (var productFromDb in productsFromDb.Where(x => stockDto.Skus.Any(t => x.Sku.Equals(t))))
                {
                    listSkuInfo.Add(new SkuInfoDto
                    {
                        Sku = productFromDb.Sku,
                        WarehouseId = stockDto.WarehouseId,
                        Items = new List<ProductDto>
                        {
                            new() {
                                Type = nameof(ProductType.FIT),
                                Count = productFromDb.Count,
                                UpdatedAt = DateTimeOffset.Now.ToString("yyyy-MM-dd'T'HH:mm:ssK")
                            }
                        }
                    });
                }
            }
            return Ok(new StocksSkuDto() { Skus = listSkuInfo });
        }
    }
}
