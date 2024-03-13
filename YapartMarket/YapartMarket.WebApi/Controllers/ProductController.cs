using AutoMapper;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using YapartMarket.Core.Data.Interfaces.Azure;
using YapartMarket.Core.DTO;
using YapartMarket.Core.DTO.Yandex;
using YapartMarket.Core.Extensions;
using YapartMarket.Core.Models.Azure;
using YapartMarket.WebApi.ViewModel;

namespace YapartMarket.React.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : Controller
    {
        readonly IConfiguration configuration;
        readonly IAzureProductRepository productRepository;
        readonly ConnectionSettings connectionSettings;
        readonly SemaphoreSlim semaphoreSlim;

        public ProductController(ConnectionSettings connectionSettings, IConfiguration configuration, IAzureProductRepository productRepository, SemaphoreSlim semaphoreSlim)
        {
            this.semaphoreSlim = semaphoreSlim ?? throw new ArgumentNullException(nameof(semaphoreSlim));
            this.connectionSettings = connectionSettings ?? throw new ArgumentNullException(nameof(connectionSettings));
            this.configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            this.productRepository = productRepository ?? throw new ArgumentNullException(nameof(productRepository));
        }


        [HttpGet]
        [Route("products")]
        [Produces("application/json")]
        public async Task<IActionResult> GetAzureProducts()
        {
            try
            {
                using (var connection = new SqlConnection(configuration.GetConnectionString("SQLServerConnectionString")))
                {
                    var sql = "select * from dbo.products";
                    await connection.OpenAsync();
                    var products = await connection.QueryAsync<Product>(sql);
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
            var cancellationToken = HttpContext?.RequestAborted ?? default;
            if (orderDto != null)
            {
                if (cancellationToken.IsCancellationRequested)
                    return BadRequest("Request aborted.");
                var isAccepted = true;
                using (var connection = new SqlConnection(configuration.GetConnectionString("SQLServerConnectionString")))
                {
                    //Пройтись по всем товарам, если хоть одного нету или количество меньше того что есть на сервере = отменить заказа
                    await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
                    foreach (var orderItem in orderDto.OrderInfoDto!.OrderItemsDto!)
                    {
                        var productInDb = await connection.QueryFirstOrDefaultAsync<Product>("select * from products where sku = @sku and count >= @count", new { sku = orderItem.OfferId, count = orderItem.Count });
                        if (productInDb == null)
                        {
                            isAccepted = false;
                            break;
                        }
                    }
                }
                return Ok(isAccepted ? 
                    new OrderViewModel { OrderInfoViewModel = new OrderInfoViewModel { Accepted = true, Id = orderDto.OrderInfoDto.Id.ToString() } } :
                    new { accepted = false, id = orderDto.OrderInfoDto.Id.ToString(), reason = Reason.OUT_OF_DATE.ToString() });
            }
            return BadRequest();
        }

        [HttpPost]
        [Route("order/status")]
        [Produces("application/json")]
        public IActionResult SetOrderStatus([FromBody] OrderDto orderStatusDto, [FromQuery(Name = "auth-token")] string authToken)
        {
            if (string.IsNullOrEmpty(authToken))
                return StatusCode(500);
            var yapartToken = configuration.GetValue<string>("auth-token");
            var yapartRogToken = configuration.GetValue<string>("auth-token-rog");
            var yapartExpressToken = configuration.GetValue<string>("auth-token-yapart-express");
            var yapartYarkoExpressToken = configuration.GetValue<string>("auth-token-yapart-yarko-express");
            if (yapartToken != authToken && yapartRogToken != authToken && yapartExpressToken != authToken && yapartYarkoExpressToken != authToken)
                return StatusCode(403);
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
            var cancellationToken = HttpContext?.RequestAborted ?? default;
            if (string.IsNullOrEmpty(authToken))
                return StatusCode(500);
            var yapartToken = configuration.GetValue<string>("auth-token");
            var yapartRogToken = configuration.GetValue<string>("auth-token-rog");
            var yapartExpressToken = configuration.GetValue<string>("auth-token-yapart-express");
            var yapartYarkoExpressToken = configuration.GetValue<string>("auth-token-yapart-yarko-express");
            if (yapartToken != authToken && yapartRogToken != authToken && yapartExpressToken != authToken && yapartYarkoExpressToken != authToken)
                return StatusCode(403);
            if (cartDto != null)
            {
                if (cartDto.Cart.CartItems != null)
                {
                    var cartViewModel = new CartViewModel
                    {
                        Cart = new CartInfoViewModel
                        {
                            CartItems = new List<CartItemViewModel>(),
                            PaymentMethods = new List<string> { "CARD_ON_DELIVERY", "CASH_ON_DELIVERY", "B2B_ACCOUNT_POSTPAYMENT" }
                        }
                    };
                    using (var connection = new SqlConnection(configuration.GetConnectionString("SQLServerConnectionString")))
                    {
                        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
                        foreach (var cartItemDto in cartDto.Cart.CartItems)
                        {
                            var count = 0;
                            var productInDb = await connection.QueryFirstOrDefaultAsync<Product>("select * from products where sku = @sku",
                                new { sku = cartItemDto.OfferId }).ConfigureAwait(false);
                            if (productInDb != null)
                            {
                                count = productInDb.Count >= cartItemDto.Count ? cartItemDto.Count : productInDb.Count;
                            }

                            cartViewModel.Cart.CartItems.Add(new CartItemViewModel
                            {
                                FeedId = cartItemDto.FeedId,
                                OfferId = cartItemDto.OfferId!,
                                Count = count
                            });
                        }
                        
                    }
                    return Ok(cartViewModel);
                }
            }
            return BadRequest();
        }
        //todo добавить доп ключ интедетификации или кейклок
        [HttpPost]
        [Route("setProducts")]
        [Produces("application/json")]
        public async Task<IActionResult> SetProductsAsync([FromBody] ItemsDto itemsDto)
        {
            var cancellationToken = HttpContext?.RequestAborted ?? default;
            try
            {
                if (cancellationToken.IsCancellationRequested)
                    return BadRequest("Request aborted.");
                if (itemsDto != null)
                {
                    var productsByInsertSkuInDb = new List<Product>();
                    using (var connection = new SqlConnection(connectionSettings.SQLServerConnectionString))
                    {
                        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
                        var takeSkus = itemsDto.Products.Select(x => x.Sku);
                        productsByInsertSkuInDb.AddRange(await connection.QueryAsync<Product>("select * from products where sku IN @skus", new { skus = takeSkus }));
                    }
                    var updateProducts = itemsDto.Products.Where(x => productsByInsertSkuInDb.Any(t => t.Sku.ToLower().Equals(x.Sku.ToLower()))).ToList();
                    var insertProducts = itemsDto.Products.Where(x => productsByInsertSkuInDb.All(t => t.Sku != x.Sku));
                    if (updateProducts.Any())
                    {
                        var result = updateProducts.Select(x => new Product
                        {
                            Sku = x.Sku,
                            Count = x.Count,
                            UpdatedAt = DateTimeOffset.Now.ToString("yyyy-MM-dd'T'HH:mm:ssK")
                        }).ToList();
                        var resultBulkUpdateMessage = await productRepository.BulkUpdateCountDataAsync(result, cancellationToken).ConfigureAwait(false);
                        if(!string.IsNullOrEmpty(resultBulkUpdateMessage))
                            return BadRequest(resultBulkUpdateMessage);
                    }
                    if (insertProducts.Any())
                    {
                        using (var connection = new SqlConnection(connectionSettings.SQLServerConnectionString))
                        {
                            await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
                            using (var transaction = await connection.BeginTransactionAsync(cancellationToken).ConfigureAwait(false))
                            {
                                foreach (var insertProduct in insertProducts)
                                {
                                    await connection.ExecuteAsync("insert into products (sku, count, updatedAt, type)  values(@sku, @count, @updatedAt, @type)", new
                                    {
                                        sku = insertProduct.Sku,
                                        count = insertProduct.Count,
                                        updatedAt = DateTimeOffset.Now.ToString("yyyy-MM-dd'T'HH:mm:ssK"),
                                        type = nameof(ProductType.FIT)
                                    }, transaction);
                                }
                                await transaction.CommitAsync(cancellationToken).ConfigureAwait(false);
                            }
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
        [HttpPost]
        [Route("setProductsExpress")]
        [Produces("application/json")]
        public async Task<IActionResult> SetProductsExpress([FromBody] ItemsDto itemsDto)
        {
            var cancellationToken = HttpContext?.RequestAborted ?? default;
            if (cancellationToken.IsCancellationRequested)
                return BadRequest("Request aborted.");
            if (itemsDto != null)
            {
                var productExpress = new ProductExpressInfoViewModel();
                try
                {
                    using (var connection = new SqlConnection(configuration.GetConnectionString("SQLServerConnectionString")))
                    {
                        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
                        var take = 2000;
                        var skip = 0;
                        var productsByInsertSkuInDb = new List<Product>();
                        do
                        {
                            var takeSkus = itemsDto.Products.Select(x => x.Sku).Skip(skip).Take(take);
                            skip += take;
                            if (!takeSkus.Any())
                                break;
                            productsByInsertSkuInDb.AddRange(await connection.QueryAsync<Product>("select * from products where sku IN @skus", new { skus = takeSkus }));
                        } while (true);
                        var missingProducts = itemsDto.Products.Where(x => productsByInsertSkuInDb.All(t => t.Sku != x.Sku));
                        if (missingProducts.IsAny())
                        {
                            productExpress.MissingProducts = missingProducts.Select(x => new ProductExpressViewModel()
                            {
                                Sku = x.Sku,
                                Count = x.Count
                            }).ToList();
                        }
                        var updateProducts = itemsDto.Products.Where(x => productsByInsertSkuInDb.Any(t => t.Sku.ToLower().Equals(x.Sku.ToLower()))).ToList();
                        if (updateProducts.Any())
                        {
                            var result = updateProducts.Select(x => new Product
                            {
                                Sku = x.Sku,
                                CountExpress = x.Count,
                                UpdateExpress = DateTimeOffset.Now.ToString("yyyy-MM-dd'T'HH:mm:ssK")
                            }).ToList();
                            await productRepository.BulkUpdateCountExpressDataAsync(result, cancellationToken);
                            productExpress.UpdateProducts = updateProducts.Select(x => new ProductExpressViewModel()
                            {
                                Sku = x.Sku,
                                Count = x.Count
                            }).ToList();
                        }
                    }
                }
                catch (Exception e)
                {
                    return BadRequest(e.Message);
                }
                return Ok(productExpress);
            }
            return BadRequest();
        }

        [HttpPost]
        [Route("stocks")]
        [Produces("application/json")]
        public async Task<IActionResult> StocksAsync([FromBody] StockDto stockDto, [FromQuery(Name = "auth-token")] string authToken)
        {
            var cancellationToken = HttpContext?.RequestAborted ?? default;
            if (stockDto == null)
                return BadRequest();
            if (string.IsNullOrEmpty(authToken))
                return StatusCode(500);
            await semaphoreSlim.WaitAsync(cancellationToken).ConfigureAwait(false);
            try
            {
                var yapartToken = configuration.GetValue<string>("auth-token");
                var yapartRogToken = configuration.GetValue<string>("auth-token-rog");
                var yapartExpressToken = configuration.GetValue<string>("auth-token-yapart-express");
                var yapartYarkoExpressToken = configuration.GetValue<string>("auth-token-yapart-yarko-express");
                if (yapartToken != authToken && yapartRogToken != authToken && yapartExpressToken != authToken && yapartYarkoExpressToken != authToken)
                    return StatusCode(403);
                var listSkuInfo = new List<SkuInfoDto>();
                using (var connection = new SqlConnection(configuration.GetConnectionString("SQLServerConnectionString")))
                {
                    await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
                    var stocksSku = stockDto.Skus;
                    var productsFromDb = await connection.QueryAsync<Product>("select * from dbo.products where sku IN @skus", new { skus = stocksSku }).ConfigureAwait(false);
                    foreach (var productFromDb in productsFromDb)
                    {
                        var currentDateTime = DateTimeOffset.Now.ToString("yyyy-MM-dd'T'HH:mm:ssK");
                        var skuInfo = new SkuInfoDto
                        {
                            Sku = productFromDb.Sku,
                            WarehouseId = 228648,
                            Items = new List<ProductDto>
                        {
                            new ProductDto
                            {
                                Type = nameof(ProductType.FIT),
                                Count = yapartYarkoExpressToken == authToken ? productFromDb.CountExpress : productFromDb.Count,
                                UpdatedAt = currentDateTime
                            }
                        }
                        };
                        listSkuInfo.Add(skuInfo);
                    }

                    if (yapartYarkoExpressToken == authToken)
                    {
                        var result = productsFromDb.Select(x => new Product
                        {
                            Sku = x.Sku,
                            TakeTimeExpress = DateTimeOffset.Now.ToString("yyyy-MM-dd'T'HH:mm:ssK")
                        }).ToList();
                        await productRepository.BulkUpdateExpressTakeTimeAsync(result).ConfigureAwait(false);
                    }
                    else
                    {
                        var result = productsFromDb.Select(x => new Product
                        {
                            Sku = x.Sku,
                            TakeTime = DateTimeOffset.Now.ToString("yyyy-MM-dd'T'HH:mm:ssK")
                        }).ToList();
                        await productRepository.BulkUpdateTakeTimeAsync(result).ConfigureAwait(false);
                    }
                }
                return Ok(new StocksSkuDto { Skus = listSkuInfo });
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
            finally
            {
                semaphoreSlim.Release();
            }
        }
    }
}