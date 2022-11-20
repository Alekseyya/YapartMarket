using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Top.Api;
using Top.Api.Request;
using Top.Api.Response;
using YapartMarket.Core.BL;
using YapartMarket.Core.Config;
using YapartMarket.Core.Data.Interfaces.Azure;
using YapartMarket.Core.DTO;
using YapartMarket.Core.DTO.AliExpress;
using YapartMarket.Core.Extensions;
using YapartMarket.Core.Models.Azure;

namespace YapartMarket.BL.Implementation
{
    public class AliExpressProductService : IAliExpressProductService // todo разбить интерфейс на части
    {
        private readonly IAzureAliExpressProductRepository _azureAliExpressProductRepository;
        private readonly IAzureProductRepository _azureProductRepository;
        private readonly IProductPropertyRepository _productPropertyRepository;
        private readonly IConfiguration _configuration;
        private readonly ILogger<AliExpressProductService> _logger;
        private readonly AliExpressOptions _aliExpressOptions;

        public AliExpressProductService(IAzureAliExpressProductRepository azureAliExpressProductRepository, IAzureProductRepository azureProductRepository,
            IProductPropertyRepository productPropertyRepository,
            IOptions<AliExpressOptions> options, IConfiguration configuration, ILogger<AliExpressProductService> logger)
        {
            _azureAliExpressProductRepository = azureAliExpressProductRepository;
            _azureProductRepository = azureProductRepository;
            _productPropertyRepository = productPropertyRepository;
            _configuration = configuration;
            _logger = logger;
            _aliExpressOptions = options.Value;
        }
        public async Task UpdateInventoryProducts(IEnumerable<Product> products)
        {
            ITopClient client = new DefaultTopClient(_aliExpressOptions.HttpsEndPoint, _aliExpressOptions.AppKey, _aliExpressOptions.AppSecret, "Json");
            var counter = 0;
            var take = 20;
            do
            {
                try
                {
                    var req = new AliexpressSolutionBatchProductInventoryUpdateRequest();
                    var takenProducts = products.Skip(counter).Take(take);
                    if (takenProducts.IsAny())
                    {
                        var reqMultipleProductUpdateList = new List<AliexpressSolutionBatchProductInventoryUpdateRequest.SynchronizeProductRequestDtoDomain>();
                        foreach (var takeProduct in takenProducts)
                        {
                            var objectProductId = new AliexpressSolutionBatchProductInventoryUpdateRequest.SynchronizeProductRequestDtoDomain();
                            reqMultipleProductUpdateList.Add(objectProductId);
                            objectProductId.ProductId = takeProduct.AliExpressProductId;
                            var synchronizeSkuRequestDtoDomains = new List<AliexpressSolutionBatchProductInventoryUpdateRequest.SynchronizeSkuRequestDtoDomain>();
                            var synchronizeSkuRequestDtoDomain = new AliexpressSolutionBatchProductInventoryUpdateRequest.SynchronizeSkuRequestDtoDomain();
                            synchronizeSkuRequestDtoDomains.Add(synchronizeSkuRequestDtoDomain);
                            synchronizeSkuRequestDtoDomain.SkuCode = takeProduct.Sku;
                            synchronizeSkuRequestDtoDomain.Inventory = takeProduct.Count;
                            objectProductId.MultipleSkuUpdateList = synchronizeSkuRequestDtoDomains;
                        }

                        req.MutipleProductUpdateList_ = reqMultipleProductUpdateList;
                        var request = client.Execute(req, _aliExpressOptions.AccessToken);
                        var body = request.Body;
                        if (body.TryParseJson(out AliExpressBatchProductInventoryUpdateResponseDTO result))
                        {
                            counter += take;
                            continue;
                        }
                        if (body.TryParseJson(out AliExpressError error))
                        {
                            _logger.LogError($"Request UpdateInventoryProducts. Message :{error.AliExpressErrorMessage.Message}. SubMessage : {error.AliExpressErrorMessage.SubMessage}");
                        }

                    }
                    else
                        break;
                }
                catch (WebException ex)
                {
                    if (ex.Status == WebExceptionStatus.Timeout || ex.Status == WebExceptionStatus.Timeout)
                    {
                        await Task.Delay(2000);
                    }
                    continue;
                }
                catch (Exception ex)
                {
                    continue;
                }
            } while (true);
        }

        public async Task ProcessDataFromAliExpress()
        {
            bool haveElement = true;
            long currentPage = 1;

            ITopClient client = new BatchTopClient(_aliExpressOptions.HttpsBatchEndPoint, _aliExpressOptions.AppKey, _aliExpressOptions.AppSecret, "Json");
            do
            {
                try
                {
                    var batch = new TopBatchRequest();
                    for (long i = currentPage; i < currentPage + 10; i++)
                    {
                        var req = new AliexpressSolutionProductListGetRequest();
                        var obj1 = new AliexpressSolutionProductListGetRequest.ItemListQueryDomain
                        {
                            CurrentPage = i,
                            ProductStatusType = "onSelling",
                            PageSize = 99,
                        };
                        req.AeopAEProductListQuery_ = obj1;
                        batch.AddRequest(req);
                    }
                    
                    var rsp = client.Execute(batch, _aliExpressOptions.AccessToken);
                    var body = rsp.Body;
                    if (body is not null)
                    {
                        if (body.Contains("-S-"))
                        {
                            var resultResponse = body.Split("-S-");
                            foreach (var product in resultResponse)
                            {
                                var listProducts = GetProductFromJson(product);
                                if (!listProducts.IsAny())
                                    haveElement = false;
                                else
                                {
                                    //Добавление новых товаров
                                    await AddNewProducts(listProducts);
                                }
                            }
                        }
                    }
                }
                catch (WebException ex)
                {
                    if (ex.Status == WebExceptionStatus.Timeout || ex.Status == WebExceptionStatus.RequestCanceled)
                    {
                        await Task.Delay(2000);
                    }

                    continue;
                }
                catch (Exception ex)
                {
                    await Task.Delay(2000);
                    continue;
                }

                currentPage += 10;
            } while (haveElement);
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

        public IReadOnlyList<ProductInfoResult> ProcessParseProductInfoBody(string body)
        {
            var productInfoResults = new List<ProductInfoResult>();
            if (body is not null)
            {
                if (body.Contains("-S-"))
                {
                    var resultResponse = body.Split("-S-");
                    productInfoResults.AddRange(DeserializeProductsInfo(resultResponse));
                }
                else
                {
                    var oneProductInfo = DeserializeProductInfo(body);
                    if (oneProductInfo != null)
                        productInfoResults.Add(oneProductInfo);
                }
            }
            return productInfoResults;
        }

        public async Task<List<ProductInfoResult>> GetProductsFromAli(IReadOnlyList<long> productIds)
        {
            var getClient = new BatchTopClient(_aliExpressOptions.HttpsBatchEndPoint, _aliExpressOptions.AppKey, _aliExpressOptions.AppSecret, "Json");
            var listProductInfo = new List<ProductInfoResult>();
            
            do
            {
                try
                {
                    var productCount = productIds.Count;
                    if (productCount < 20)
                    {
                        var batch = new TopBatchRequest();
                        foreach (var productId in productIds)
                        {
                            var requestProductInfo = new AliexpressSolutionProductInfoGetRequest
                            {
                                ProductId = productId
                            };
                            batch.AddRequest(requestProductInfo);
                        }
                    }

                    if (productCount > 20)
                    {
                        var cycles = productCount / 20;
                        var remainderCycles = productCount % 20 > 0 ? productCount - (cycles * 20) : 0;
                        for (int cycle = 0; cycle < cycles; cycle++)
                        {
                            var batch = new TopBatchRequest();
                            foreach (var productId in productIds.Skip(cycle * 20).Take(20).ToList())
                            {
                                var requestProductInfo = new AliexpressSolutionProductInfoGetRequest
                                {
                                    ProductId = productId
                                };
                                batch.AddRequest(requestProductInfo);
                            }
                            var productInfoResponse = getClient.Execute(batch, _aliExpressOptions.AccessToken);
                            listProductInfo.AddRange(ProcessParseProductInfoBody(productInfoResponse.Body));
                        }

                        if (remainderCycles > 0)
                        {
                            var batch = new TopBatchRequest();
                            foreach (var productId in productIds.Skip(cycles * 20).Take(remainderCycles).ToList())
                            {
                                var requestProductInfo = new AliexpressSolutionProductInfoGetRequest
                                {
                                    ProductId = productId
                                };
                                batch.AddRequest(requestProductInfo);
                            }
                            var productInfoResponse = getClient.Execute(batch, _aliExpressOptions.AccessToken);
                            listProductInfo.AddRange(ProcessParseProductInfoBody(productInfoResponse.Body));
                        }
                    }
                }
                catch (WebException ex)
                {
                    if (ex.Status == WebExceptionStatus.Timeout)
                    {
                        await Task.Delay(3000);
                    }
                    continue;
                }
                catch (Exception ex)
                {
                    await Task.Delay(3000);
                    continue;
                }
                break;
            } while (true);
            return listProductInfo;
        }
        public async Task ProcessUpdateProductsSku()
        {
            await _azureAliExpressProductRepository.Delete("DELETE FROM aliExpressProducts; DBCC CHECKIDENT('aliExpressProducts', RESEED, 0);");
            await ProcessDataFromAliExpress();
            var aliExpressProducts = await _azureAliExpressProductRepository.GetAsync("select * from dbo.aliExpressProducts where updatedAt <= @updatedAt or sku is null",
                new { updatedAt = DateTimeOffset.Now.AddDays(-1).ToString("yyyy-MM-dd'T'HH:mm:ssK") });
            if (aliExpressProducts != null)
            {
                var countCycles = aliExpressProducts!.Count()/1000;
                var remaider = aliExpressProducts!.Count() % 1000;
                for (int cycle = 0; cycle < countCycles; cycle++)
                {
                    await ProcessUpdateProducts(aliExpressProducts.Select(x => x.ProductId!.Value).Skip(cycle * 1000).Take(1000).ToList());
                }
                if(remaider > 0)
                    await ProcessUpdateProducts(aliExpressProducts.Select(x => x.ProductId!.Value).Skip(countCycles * 1000).Take(remaider).ToList());
            }
        }
        public async Task ProcessUpdateProducts(IReadOnlyList<long> productIds)
        {
            var productsInDb = (await _azureAliExpressProductRepository.GetInAsync("productId", new { productId = productIds.Select(x => x) })).AsList();
            if (productsInDb.IsAny())
            {
                var productsInfo = await GetProductsFromAli(productIds);
                await ModifiedProducts(productsInDb, productsInfo);
                await ModifiedProductProperties(productsInfo);
            }
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
        public async Task ProcessUpdateProduct(long productId)
        {
            var productInDb = await _azureAliExpressProductRepository.GetAsync("select * from dbo.aliExpressProducts where productId =@productId", new {productId = productId});
            if (productInDb.IsAny())
            {
                try
                {
                    var productsInfo = await GetProductsFromAli(productInDb.Select(x => x.ProductId.Value).ToList());
                    var productInfoDb = await _azureAliExpressProductRepository.GetInAsync("productId", new { productId = productsInfo.Select(x => x.ProductId) });

                    await ModifiedProducts(productInfoDb.ToList(), productsInfo);
                    await ModifiedProductProperties(productsInfo);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
        public async Task AddNewProducts(IEnumerable<AliExpressProductDTO> products)
        {
            var productsInDb = await _azureAliExpressProductRepository.GetInAsync(nameof(AliExpressProduct.ProductId), new {ProductId = products.Select(x=>x.ProductId)});
            try
            {
                var newProducts = products.Where(prod => productsInDb.All(prodDb => prodDb.ProductId == 0 || prodDb.ProductId != prod.ProductId));
                if (newProducts.IsAny())
                {
                    var commands = new List<object>();
                    foreach (var newProduct in newProducts)
                    {
                        commands.Add(new
                        {
                            productId = newProduct.ProductId,
                            created = DateTimeOffset.Now.ToString("yyyy-MM-dd'T'HH:mm:ssK")
                        });
                    }
                    await _azureAliExpressProductRepository.InsertAsync("insert into dbo.aliExpressProducts(productId, created) values(@productId, @created)", commands);
                }
            }
            catch (Exception)
            {
                throw;
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
                        prodDb => prodDb.Sku, (aliExpProd, prodDb) => new {prodDb, aliExpProd});
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
        public ProductInfoResult GetProductInfo(long productId)
        {
            ITopClient client = new DefaultTopClient(_aliExpressOptions.HttpsEndPoint, _aliExpressOptions.AppKey, _aliExpressOptions.AppSecret, "Json");
            AliexpressSolutionProductInfoGetRequest req = new AliexpressSolutionProductInfoGetRequest
            {
                ProductId = productId
            };
            AliexpressSolutionProductInfoGetResponse rsp = client.Execute(req, _aliExpressOptions.AccessToken);
            var result = JsonConvert.DeserializeObject<ProductInfoRoot>(rsp.Body);
            return result?.Response.ProductInfoResult;
        }

        private IEnumerable<AliExpressProductDTO> GetProductFromJson(string json)
        {
            var jsonObject = JObject.Parse(json);
            var listProductDtos = jsonObject.SelectToken("aliexpress_solution_product_list_get_response.result.aeop_a_e_product_display_d_t_o_list.item_display_dto")?.ToObject<IEnumerable<AliExpressProductDTO>>();
            return listProductDtos;
        }
        //todo переписать на объект сериализации!!
        public AliExpressProductDTO ProductStringToDTO(string json)
        {
            var jsonObject = JObject.Parse(json);
            var productJson = jsonObject.SelectToken("aliexpress_solution_product_info_get_response.result.aeop_ae_product_s_k_us.global_aeop_ae_product_sku")?[0]?.ToString();
            try
            {
                if (!string.IsNullOrEmpty(productJson))
                {
                    var aliExpressProduct = JsonConvert.DeserializeObject<AliExpressProductDTO>(productJson);
                    var productId = (long) jsonObject.SelectToken("aliexpress_solution_product_info_get_response.result.product_id");
                    var description = jsonObject.SelectToken("aliexpress_solution_product_info_get_response.result.subject")?.ToString();
                    if (aliExpressProduct != null)
                    {
                        aliExpressProduct.ProductId = productId;
                        aliExpressProduct.Description = description;
                        return aliExpressProduct;
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return null;
        }
    }
}
