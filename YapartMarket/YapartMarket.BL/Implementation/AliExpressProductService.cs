using System;
using System.Data;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Collections.Generic;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using YapartMarket.Core.BL;
using YapartMarket.Core.DTO;
using YapartMarket.Core.Config;
using YapartMarket.Core.Models.Raw;
using YapartMarket.Core.Models.Azure;
using YapartMarket.Core.DTO.AliExpress;
using YapartMarket.Core.Data.Interfaces.Azure;
using Product = YapartMarket.Core.Models.Azure.Product;

namespace YapartMarket.BL.Implementation
{
    public class AliExpressProductService : SendRequest<ProductRoot>, IAliExpressProductService
    {
        private readonly IAzureProductRepository _azureProductRepository;
        private readonly IConfiguration _configuration;
        private readonly ILogger<AliExpressProductService> _logger;
        private readonly AliExpressOptions _aliExpressOptions;
        private readonly HttpClient _httpClient;

        public AliExpressProductService(IAzureProductRepository azureProductRepository,
            IOptions<AliExpressOptions> options, IConfiguration configuration, ILogger<AliExpressProductService> logger, IHttpClientFactory factory)
        {
            _azureProductRepository = azureProductRepository;
            _configuration = configuration;
            _logger = logger;
            _aliExpressOptions = options.Value;
            _httpClient = factory.CreateClient("aliExpress");
            _httpClient.DefaultRequestHeaders.Add("x-auth-token", options.Value.AuthToken);
        }
        public async Task ProcessUpdateProductSkuAsync()
        {
            var products = new List<Product>();
            using (var connection = new SqlConnection(_configuration.GetConnectionString("SQLServerConnectionString")))
            {
                await connection.OpenAsync().ConfigureAwait(false);
                var productsInDb = await connection.QueryAsync<Product>("select * FROM dbo.products");
                products.AddRange(productsInDb);
            }
            var aliProductRequest = new AliProductRequest(_httpClient);
            var productResponses = await aliProductRequest.Send(products, _aliExpressOptions.GetProducts!);
            using (var connection = new SqlConnection(_configuration.GetConnectionString("SQLServerConnectionString")))
            {
                await connection.OpenAsync().ConfigureAwait(false);
                using (var transaction = await connection.BeginTransactionAsync())
                {
                    var updateSql = @"update products set aliExpressProductId = @aliExpressProductId where sku = @sku;";
                    foreach (var responce in productResponses)
                    {
                        foreach (var product in responce.data!)
                        {
                            var sku = product.sku!.FirstOrDefault()!.code;
                            var aliProductId = product.id;
                            await connection.ExecuteAsync(updateSql, new { aliExpressProductId = aliProductId, sku = sku }, transaction).ConfigureAwait(false);
                        }
                    }
                    await transaction.CommitAsync().ConfigureAwait(false);
                }
            }
        }

        public async Task<UpdateStocksResponse> UpdateProductAsync(IReadOnlyList<Product> products)
        {
            var productsResult = new ProductRoot()
            {
                products = new List<Core.Models.Raw.Product>()
            };
            foreach (var product in products)
            {
                productsResult.products.Add(new()
                {
                    product_id = product.AliExpressProductId.ToString()!,
                    skus = new List<Sku>()
                    {
                        new Sku()
                        {
                            sku_code = product.Sku,
                            inventory = product.Count.ToString()
                        }
                    }
                });
            }

            var skip = 0;
            var count = products.Count;
            var response = new UpdateStocksResponse();
            while (skip < count)
            {
                var tmpProduct = new ProductRoot()
                {
                    products = productsResult.products.Skip(skip).Take(500).ToList()
                };
                var result = await Request(tmpProduct, _aliExpressOptions.UpdateStocks!, _httpClient);
                var responseTmp = JsonConvert.DeserializeObject<UpdateStocksResponse>(result);
                if (responseTmp != null && responseTmp.results != null && responseTmp.results.Any(x => !x.ok))
                    response.results!.AddRange(responseTmp.results.Where(x => !x.ok).ToList());
                skip += 500;
            }
            return response;
        }
        public async Task<UpdateStocksResponse> ProcessUpdateStocksAsync()
        {
            var products = new List<Product>();
            using (var connection = new SqlConnection(_configuration.GetConnectionString("SQLServerConnectionString")))
            {
                await connection.OpenAsync();
                var productsInDb = await connection.QueryAsync<Product>("select * from products p WHERE p.aliExpressProductId is not null;");
                products = productsInDb.ToList();
            }
            return await UpdateProductAsync(products);
        }

        public IReadOnlyList<ProductInfoResult> DeserializeProductsInfo(IReadOnlyList<string> responseProducts)
        {
            var listProductInfo = new List<ProductInfoResult>();
            foreach (var responseProduct in responseProducts)
            {
                var productInfo = DeserializeProductInfo(responseProduct);
                if (productInfo != null && productInfo.ProductId != 0)
                    listProductInfo.Add(productInfo);
            }
            return listProductInfo;
        }

        public ProductInfoResult DeserializeProductInfo(string responseProduct)
        {
            var productInfo = JsonConvert.DeserializeObject<ProductInfoRoot>(responseProduct)?.Response?.ProductInfoResult;
            return productInfo!;
        }

        public async Task<IEnumerable<Product>> ListProductsForUpdateInventoryAsync()
        {
            using (var connection = new SqlConnection(_configuration.GetConnectionString("SQLServerConnectionString")))
            {
                await connection.OpenAsync();
                var productsInDb = await connection.QueryAsync<Product, AliExpressProduct, Product>(
                    "select * FROM dbo.products p inner join dbo.aliExpressProducts aep on p.sku = aep.sku",
                    (product, aliExpressProduct) =>
                    {
                        product.AliExpressProduct = aliExpressProduct;
                        return product;
                    }, splitOn: "productId");
                return productsInDb;
            }
        }
        public async Task<IEnumerable<AliExpressProductDTO>?> ExceptProductsFromDataBaseAsync(IEnumerable<AliExpressProductDTO> products)
        {
            if (products.Any())
            {
                IEnumerable<Product> productsInDb;
                using (var connection = new SqlConnection(_configuration.GetConnectionString("SQLServerConnectionString")))
                {
                    await connection.OpenAsync();
                    productsInDb = await connection.QueryAsync<Product>("select * from products where aliExpressProductId IN @aliExpressProductIds", new { aliExpressProductIds = products.Select(x => x.ProductId) });
                }
                return products.Where(prod => productsInDb.All(prodDb => prodDb.Sku != prod.SkuCode));
            }
            return null;
        }
        public async Task<List<AliExpressProductDTO>?> SetInventoryFromDatabaseAsync(List<AliExpressProductDTO> aliExpressProducts)
        {
            if (aliExpressProducts.Any())
            {
                var newAliExpressProduct = new List<AliExpressProductDTO>();
                newAliExpressProduct = aliExpressProducts;
                IEnumerable<Product>? productsInDb = null;
                using (var connection = new SqlConnection(_configuration.GetConnectionString("SQLServerConnectionString")))
                {
                    await connection.OpenAsync().ConfigureAwait(false);
                    productsInDb = await connection.QueryAsync<Product>("select * from products where sku IN @skus", new { skus = aliExpressProducts.Select(x => x.SkuCode) }).ConfigureAwait(false);
                }

                if (productsInDb.Any())
                {
                    var pairs = newAliExpressProduct.Join(productsInDb, aliExpProd => aliExpProd.SkuCode,
                        prodDb => prodDb.Sku, (aliExpProd, prodDb) => new { prodDb, aliExpProd });
                    foreach (var pair in pairs)
                    {
                        pair.aliExpProd.Inventory = pair.prodDb.Count;
                    }
                    return newAliExpressProduct;
                }
            }
            return null;
        }

        public async Task ProcessUpdateDatabaseAliExpressProductIdAsync()
        {
            var updateProducts = await GetProductWhereAliExpressProductIdIsNullAsync();
            if (updateProducts.Any())
                await _azureProductRepository.BulkUpdateProductIdAsync(updateProducts.ToList());
        }

        public async Task<IEnumerable<Product>> GetProductWhereAliExpressProductIdIsNullAsync()
        {
            using (var connection = new SqlConnection(_configuration.GetConnectionString("SQLServerConnectionString")))
            {
                await connection.OpenAsync().ConfigureAwait(false);
                _logger.LogInformation("Связывание таблиц AliExpressProduct с Product");
                var lookup = new Dictionary<int, Product>();
                var productsInDb = await connection.QueryAsync<Product, AliExpressProduct, Product>(
                    "select * FROM dbo.products p inner join dbo.aliExpressProducts aep on p.sku = aep.sku",
                    (p, a) =>
                    {
                        p.AliExpressProduct = a;
                        p.AliExpressProductId = a.ProductId;
                        return p;
                    }, splitOn: "productId");
                _logger.LogInformation($"Количество записей, которые стоит обновить {productsInDb.Count()}");
                return productsInDb;
            }
        }
    }
}
