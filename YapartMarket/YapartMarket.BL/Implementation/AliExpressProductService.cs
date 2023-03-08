using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using YapartMarket.Core.BL;
using YapartMarket.Core.Config;
using YapartMarket.Core.Data.Interfaces.Azure;
using YapartMarket.Core.DTO;
using YapartMarket.Core.DTO.AliExpress;
using YapartMarket.Core.Extensions;
using YapartMarket.Core.Models.Azure;
using YapartMarket.Core.Models.Raw;
using Product = YapartMarket.Core.Models.Azure.Product;

namespace YapartMarket.BL.Implementation
{
    public class AliExpressProductService : SendRequest<ProductRoot>, IAliExpressProductService
    {
        private readonly IAzureAliExpressProductRepository _azureAliExpressProductRepository;
        private readonly IAzureProductRepository _azureProductRepository;
        private readonly IProductPropertyRepository _productPropertyRepository;
        private readonly IConfiguration _configuration;
        private readonly ILogger<AliExpressProductService> _logger;
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly AliExpressOptions _aliExpressOptions;
        private readonly HttpClient _httpClient;

        public AliExpressProductService(IAzureAliExpressProductRepository azureAliExpressProductRepository, IAzureProductRepository azureProductRepository,
            IProductPropertyRepository productPropertyRepository,
            IOptions<AliExpressOptions> options, IConfiguration configuration, ILogger<AliExpressProductService> logger,
            IServiceScopeFactory scopeFactory, IHttpClientFactory factory)
        {
            _azureAliExpressProductRepository = azureAliExpressProductRepository;
            _azureProductRepository = azureProductRepository;
            _productPropertyRepository = productPropertyRepository;
            _configuration = configuration;
            _logger = logger;
            _scopeFactory = scopeFactory;
            _aliExpressOptions = options.Value;
            _httpClient = factory.CreateClient("aliExpress");
            _httpClient.DefaultRequestHeaders.Add("x-auth-token", options.Value.AuthToken);
        }
        public async Task ProcessUpdateProductSku()
        {
            var products = new List<Product>();
            using (var connection = new SqlConnection(_configuration.GetConnectionString("SQLServerConnectionString")))
            {
                await connection.OpenAsync();
                var productsInDb = await connection.QueryAsync<Product>("select * FROM dbo.products");
                products.AddRange(productsInDb);
            }
            var aliProductRequest = new AliProductRequest(_httpClient);
            var productResponses = await aliProductRequest.Send(products, _aliExpressOptions.GetProducts);
            using (var connection = new SqlConnection(_configuration.GetConnectionString("SQLServerConnectionString")))
            {
                await connection.OpenAsync();
                using (var transaction = await connection.BeginTransactionAsync())
                {
                    var updateSql = @"update products set aliExpressProductId = @aliExpressProductId where sku = @sku;";
                    foreach (var responce in productResponses)
                    {
                        foreach (var product in responce.data)
                        {
                            var sku = product.sku.FirstOrDefault().code;
                            var aliProductId = product.id;
                            await connection.ExecuteAsync(updateSql, new { aliExpressProductId = aliProductId, sku = sku }, transaction).ConfigureAwait(false);
                        }
                    }
                    await transaction.CommitAsync().ConfigureAwait(false);
                }
            }
        }

        public async Task<UpdateStocksResponse> UpdateProduct(IReadOnlyList<Product> products)
        {
            var productsResult = new ProductRoot()
            {
                products = new List<Core.Models.Raw.Product>()
            };
            foreach (var product in products)
            {
                productsResult.products.Add(new()
                {
                    product_id = product.AliExpressProductId.ToString(),
                    skus = new List<Core.Models.Raw.Sku>()
                    {
                        new Core.Models.Raw.Sku()
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
                try
                {
                    var tmpProduct = new ProductRoot()
                    {
                        products = productsResult.products.Skip(skip).Take(500).ToList()
                    };
                    var result = await Request(tmpProduct, _aliExpressOptions.UpdateStocks, _httpClient);
                    var responseTmp = JsonConvert.DeserializeObject<UpdateStocksResponse>(result);
                    if (responseTmp != null && responseTmp.results != null && responseTmp.results.Any(x => !x.ok))
                        response.results.AddRange(responseTmp.results.Where(x => !x.ok).ToList());
                }
                catch (Exception e)
                {
                    throw e;
                }
                skip += 500;
            }
            return response;
        }
        public async Task<UpdateStocksResponse> ProcessUpdateStocks()
        {
            var products = new List<Product>();
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
                products.AddRange(productsInDb);
            }
            return await UpdateProduct(products);
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
            return productInfo;
        }


        public async Task ModifiedProducts(IReadOnlyList<AliExpressProduct> products, IReadOnlyList<ProductInfoResult> productsInfo)
        {
            var modifiesProducts = productsInfo.Where(x =>
            {
                var currentCode = x.ProductInfoSku?.GlobalProductSkus?.FirstOrDefault()?.CurrencyCode;
                var skuCode = x.ProductInfoSku?.GlobalProductSkus?.FirstOrDefault()?.SkuCode;
                var productPrice = decimal.Parse(x.ProductPrice, CultureInfo.InvariantCulture);
                return products.Any(t => t.ProductId == x.ProductId &&
                                         (t.CategoryId != x.CategoryId ||
                                          t.ProductUnit != x.ProductUnit ||
                                          t.CurrencyCode != currentCode ||
                                          t.GroupId != x.GroupId ||
                                          t.GrossWeight != x.GrossWeight) ||
                                         t.PackageHeight != x.PackageHeight ||
                                         t.PackageLength != x.PackageLength ||
                                         t.ProductPrice != productPrice ||
                                         t.ProductStatusType != x.ProductStatusType ||
                                         t.Sku != skuCode);
            });
            if (modifiesProducts.IsAny())
            {
                var updateList = productsInfo.Select(x => new AliExpressProduct()
                {
                    Sku = x.ProductInfoSku?.GlobalProductSkus?.FirstOrDefault()?.SkuCode,
                    CategoryId = x.CategoryId,
                    CurrencyCode = x.ProductInfoSku?.GlobalProductSkus?.FirstOrDefault()?.CurrencyCode,
                    GroupId = x.GroupId,
                    GrossWeight = x.GrossWeight,
                    PackageHeight = x.PackageHeight,
                    PackageLength = x.PackageLength,
                    PackageWidth = x.PackageWidth,
                    ProductPrice = decimal.Parse(x.ProductPrice, CultureInfo.InvariantCulture),
                    ProductStatusType = x.ProductStatusType,
                    ProductUnit = x.ProductUnit,
                    UpdatedAt = DateTimeOffset.Now.ToString("yyyy-MM-dd'T'HH:mm:ssK"),
                    ProductId = x.ProductId

                });
                await _azureAliExpressProductRepository.BulkUpdateData(updateList.ToList());
            }
        }
        public async Task ModifiedProductProperties(IReadOnlyList<ProductInfoResult> productsInfo)
        {
            foreach (var productInfo in productsInfo)
            {
                var globalProperties = productInfo.ProductInfoProperties.GlobalProductProperties;
                if (globalProperties.Any())
                {
                    var productPropertiesInfoDb = await _productPropertyRepository.GetAsync("select * from ali_product_properties where product_id = @product_id", new { product_id = productInfo.ProductId });
                    var modifiedProperties = globalProperties.Where(x => productPropertiesInfoDb.Any(t =>
                        t.ProductId == productInfo.ProductId && t.AttributeName != x.AttributeName &&
                        t.AttributeNameId != x.AttributeNameId && t.AttributeValue != x.AttributeValue));
                    if (modifiedProperties.IsAny())
                    {
                        var updateProductPropertiesSql = new ProductProperty().UpdateString("dbo.ali_product_properties", "product_id = @product_id");
                        await _productPropertyRepository.Update(updateProductPropertiesSql, modifiedProperties.Select(x => new
                        {
                            product_id = productInfo.ProductId,
                            attr_name = x.AttributeName,
                            attr_name_id = x.AttributeNameId,
                            attr_value = x.AttributeValue
                        }));
                    }
                    if (!productPropertiesInfoDb.IsAny())
                    {
                        var insertProductPropertySql = new ProductProperty().InsertString("dbo.ali_product_properties");
                        await _productPropertyRepository.InsertAsync(insertProductPropertySql,
                            globalProperties.Select(x => new
                            {
                                product_id = productInfo.ProductId,
                                attr_name = x.AttributeName,
                                attr_name_id = x.AttributeNameId,
                                attr_value = x.AttributeValue
                            }));
                    }
                }
            }
        }
        public async Task<IEnumerable<Product>> ListProductsForUpdateInventory()
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
        public async Task<IEnumerable<AliExpressProductDTO>> ExceptProductsFromDataBase(IEnumerable<AliExpressProductDTO> products)
        {
            if (products.Any())
            {
                //todo в отдельный 
                IEnumerable<Product> productsInDb;
                using (var connection = new SqlConnection(_configuration.GetConnectionString("SQLServerConnectionString")))
                {
                    connection.Open();
                    productsInDb = await connection.QueryAsync<Product>("select * from products where aliExpressProductId IN @aliExpressProductIds", new { aliExpressProductIds = products.Select(x => x.ProductId) });
                }
                return products.Where(prod => productsInDb.All(prodDb => prodDb.Sku != prod.SkuCode));
            }
            return null;
        }
        public List<AliExpressProductDTO> SetInventoryFromDatabase(List<AliExpressProductDTO> aliExpressProducts)
        {
            if (aliExpressProducts.Any())
            {
                var newAliExpressProduct = new List<AliExpressProductDTO>();
                newAliExpressProduct = aliExpressProducts;
                IEnumerable<Product> productsInDb = null;
                using (var connection = new SqlConnection(_configuration.GetConnectionString("SQLServerConnectionString")))
                {
                    connection.Open();
                    productsInDb = connection.Query<Product>("select * from products where sku IN @skus", new { skus = aliExpressProducts.Select(x => x.SkuCode) });
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
                return null;
            }
            return null;
        }

        public async Task ProcessUpdateDatabaseAliExpressProductId()
        {
            var updateProducts = await GetProductWhereAliExpressProductIdIsNull();
            if (updateProducts.Any())
                await _azureProductRepository.BulkUpdateProductId(updateProducts.ToList());
        }

        public async Task<IEnumerable<Product>> GetProductWhereAliExpressProductIdIsNull()
        {
            using (var connection = new SqlConnection(_configuration.GetConnectionString("SQLServerConnectionString")))
            {
                await connection.OpenAsync();
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
