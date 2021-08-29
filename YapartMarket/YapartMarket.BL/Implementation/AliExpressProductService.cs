using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Data.SqlClient;
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
using YapartMarket.Core.Extensions;
using YapartMarket.Core.Models.Azure;

namespace YapartMarket.BL.Implementation
{
    public class AliExpressProductService : IAliExpressProductService // todo разбить интерфейс на части
    {
        private readonly IAzureAliExpressProductRepository _azureAliExpressProductRepository;
        private readonly IAzureProductRepository _azureProductRepository;
        private readonly IConfiguration _configuration;
        private readonly ILogger<AliExpressProductService> _logger;
        private readonly AliExpressOptions _aliExpressOptions;

        public AliExpressProductService(IAzureAliExpressProductRepository azureAliExpressProductRepository, IAzureProductRepository azureProductRepository, IOptions<AliExpressOptions> options, IConfiguration configuration, ILogger<AliExpressProductService> logger)
        {
            _azureAliExpressProductRepository = azureAliExpressProductRepository;
            _azureProductRepository = azureProductRepository;
            _configuration = configuration;
            _logger = logger;
            _aliExpressOptions = options.Value;
        }
        public void UpdateInventoryProducts(IEnumerable<AliExpressProductDTO> aliExpressProducts)
        {
            try
            {
                ITopClient client = new DefaultTopClient(_aliExpressOptions.HttpsEndPoint, _aliExpressOptions.AppKey, _aliExpressOptions.AppSecret, "Json");
                AliexpressSolutionBatchProductInventoryUpdateRequest req = new AliexpressSolutionBatchProductInventoryUpdateRequest();
                for (int i = 0; i < aliExpressProducts.Count(); i += 20)
                {
                    var takeProducts = aliExpressProducts.Skip(i).Take(20);
                    if (takeProducts.Any())
                    {
                        var reqMultipleProductUpdateList = new List<AliexpressSolutionBatchProductInventoryUpdateRequest.SynchronizeProductRequestDtoDomain>();
                        foreach (var takeProduct in takeProducts)
                        {
                            var objectProductId = new AliexpressSolutionBatchProductInventoryUpdateRequest.SynchronizeProductRequestDtoDomain();
                            reqMultipleProductUpdateList.Add(objectProductId);
                            objectProductId.ProductId = takeProduct.ProductId;
                            var synchronizeSkuRequestDtoDomains = new List<AliexpressSolutionBatchProductInventoryUpdateRequest.SynchronizeSkuRequestDtoDomain>();
                            var synchronizeSkuRequestDtoDomain = new AliexpressSolutionBatchProductInventoryUpdateRequest.SynchronizeSkuRequestDtoDomain();
                            synchronizeSkuRequestDtoDomains.Add(synchronizeSkuRequestDtoDomain);
                            synchronizeSkuRequestDtoDomain.SkuCode = takeProduct.SkuCode;
                            synchronizeSkuRequestDtoDomain.Inventory = takeProduct.Inventory;
                            objectProductId.MultipleSkuUpdateList = synchronizeSkuRequestDtoDomains;
                        }

                        req.MutipleProductUpdateList_ = reqMultipleProductUpdateList;
                        var request = client.Execute(req, _aliExpressOptions.AccessToken);
                        if (request.Body.TryParseJson(out AliExpressBatchProductInventoryUpdateResponseDTO responseDto))
                        {
                            //todo записать в лог ошибку
                        }

                        if (request.Body.TryParseJson(out AliExpressError error)){}
                        //todo записать в лог ошибку

                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        
        public async Task ProcessDataFromAliExpress()
        {
            bool haveElement = true;
            long currentPage = 1;
            var newProducts = new List<AliExpressProductDTO>();
            try
            {
                ITopClient client = new DefaultTopClient(_aliExpressOptions.HttpsEndPoint, _aliExpressOptions.AppKey, _aliExpressOptions.AppSecret, "Json");
                do
                {
                    _logger.LogInformation($"Запрос. Страница {currentPage}");
                    var req = new AliexpressSolutionProductListGetRequest();
                    var obj1 = new AliexpressSolutionProductListGetRequest.ItemListQueryDomain
                    {
                        CurrentPage = currentPage,
                        ProductStatusType = "onSelling",
                        PageSize = 99
                    };
                    req.AeopAEProductListQuery_ = obj1;
                    var rsp = client.Execute(req, _aliExpressOptions.AccessToken);
                    _logger.LogInformation($"Страница {currentPage} Десериализация json продуктов");
                    var listProducts = GetProductFromJson(rsp.Body);
                    if (!listProducts.IsAny())
                        haveElement = false;
                    else
                    {
                        //Добавлени новых товаров
                        var tmpNewProducts = await AddNewProducts(listProducts);
                        if (tmpNewProducts != null && tmpNewProducts.Any())
                            newProducts.AddRange(tmpNewProducts);
                    }
                    currentPage++;
                } while (haveElement);

                var productsInDbEmptySku = await _azureAliExpressProductRepository.GetAsync("select * from aliExpressProducts where sku is null");
                //обновить у них SKU
                if (productsInDbEmptySku.IsAny())
                {
                    ITopClient getClient = new DefaultTopClient(_aliExpressOptions.HttpsEndPoint, _aliExpressOptions.AppKey, _aliExpressOptions.AppSecret, "Json");
                    foreach (var product in productsInDbEmptySku)
                    {
                        var requestProductInfo = new AliexpressSolutionProductInfoGetRequest
                        {
                            ProductId = product.ProductId
                        };
                        var productInfoResponse = getClient.Execute(requestProductInfo, _aliExpressOptions.AccessToken);
                        var productInfo = ProductStringToDTO(productInfoResponse.Body); //read Json
                        if (productInfo != null)
                        {
                            product.SKU = productInfo.SkuCode;
                            product.Inventory = productInfo.Inventory;
                        }
                    }

                    foreach (var product in productsInDbEmptySku)
                    {
                        await _azureAliExpressProductRepository.Update(
                            "update aliExpressProducts set sku = @sku, inventory = @inventory, updatedAt = @updatedAt where productId = @productId",
                            new
                            {
                                sku = product.SKU,
                                inventory = product.Inventory,
                                updateAt = DateTimeOffset.Now.ToString("yyyy-MM-dd'T'HH:mm:ssK"),
                                productId = product.ProductId
                            });
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<AliExpressProductDTO>> AddNewProducts(IEnumerable<AliExpressProductDTO> products)
        {
            var productsInDb = await _azureAliExpressProductRepository.GetInAsync(nameof(AliExpressProduct.ProductId), new {ProductId = products.Select(x=>x.ProductId)});
            //новые записи
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
                    return newProducts;
                }
            }
            catch (Exception)
            {
                throw;
            }
            return null;
        }

        //Подрефакторить
        public IEnumerable<AliExpressProductDTO> GetProductsAliExpress(Expression<Func<AliExpressProductDTO, bool>> conditionFunction = null)
        {
            var listProducts = new List<AliExpressProductDTO>();
            bool haveElement = true;
            long currentPage = 1;
            try
            {
                //todo в отдельный сервис - ProductInfoALiExpress
                ITopClient client = new DefaultTopClient(_aliExpressOptions.HttpsEndPoint, _aliExpressOptions.AppKey,
                    _aliExpressOptions.AppSecret, "Json");
                do
                {
                    _logger.LogInformation($"Запрос. Страница {currentPage}");
                    var req = new AliexpressSolutionProductListGetRequest();
                    var obj1 = new AliexpressSolutionProductListGetRequest.ItemListQueryDomain
                    {
                        CurrentPage = currentPage,
                        ProductStatusType = "onSelling",
                        PageSize = 99
                    };
                    req.AeopAEProductListQuery_ = obj1;
                    var rsp = client.Execute(req, _aliExpressOptions.AccessToken);
                    _logger.LogInformation($"Страница {currentPage} Десериализация json продуктов");
                    var tmpListProductFromJson = GetProductFromJson(rsp.Body);
                    if (!tmpListProductFromJson.Any())
                        haveElement = false;
                    else
                    {
                        //обновление коллекции полем SKUCode
                        _logger.LogInformation("Обновление информации о SKU");
                        tmpListProductFromJson = UpdateSkuFromAliExpress(tmpListProductFromJson);
                        _logger.LogInformation("Обновление количества продукта");
                        tmpListProductFromJson = SetInventoryFromDatabase(tmpListProductFromJson.ToList()); //todo по сути Обновление полей у AliExpressProductDTO
                        listProducts.AddRange(tmpListProductFromJson);
                    }

                    currentPage++;
                } while (haveElement);

                if (conditionFunction != null)
                    return listProducts.AsQueryable().Where(conditionFunction).AsEnumerable();
                return listProducts.AsEnumerable();
            }
            catch (AggregateException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogDebug(ex.Message);
                if (conditionFunction != null)
                   return listProducts.AsQueryable().Where(conditionFunction).AsEnumerable();
                return listProducts.AsEnumerable();
            }
        }

        private IEnumerable<T> UpdateSkuFromAliExpress<T>(IEnumerable<T> products) where T : AliExpressProductDTO
        {
            if (products.Any())
            {
                ReturnProductNotFoundDatabase(products, out IEnumerable<T> intersectProducts, out IEnumerable<T> exceptProducts);
                ITopClient client = new DefaultTopClient(_aliExpressOptions.HttpsEndPoint, _aliExpressOptions.AppKey, _aliExpressOptions.AppSecret, "Json");
                foreach (var expressProduct in exceptProducts)
                {
                    var reqInfoProduct = new AliexpressSolutionProductInfoGetRequest
                    {
                        ProductId = expressProduct.ProductId
                    };
                    var executeProductInfo = client.Execute(reqInfoProduct, _aliExpressOptions.AccessToken);
                    var tmpInfoProduct = ProductStringToDTO(executeProductInfo.Body);
                    expressProduct.SkuCode = tmpInfoProduct?.SkuCode;
                }

                ((List<T>)intersectProducts).AddRange(exceptProducts);
                return intersectProducts;
            }
            return null;
        }
        //todo переименовать метод
        private void ReturnProductNotFoundDatabase<T>(IEnumerable<T> products, out IEnumerable<T> intersectProducts, out IEnumerable<T> exceptProducts) where T : AliExpressProductDTO
        {
            IEnumerable<Product> productsInDb = null;
            if (products.Any())
            {
                //todo нужен будет Await!!!
                productsInDb = Task.Run(() => _azureProductRepository.GetInAsync(nameof(Product.AliExpressProductId), new { AliExpressProductId = products.Select(x => x.ProductId) })).GetAwaiter().GetResult();
                if (productsInDb.Any())
                {
                    //проставить sku в aliExpressProducts
                    var pairs = products.Join(productsInDb, aliExpProd => aliExpProd.ProductId,
                        prodDb => prodDb.AliExpressProductId, (aliExpProd, prodDb) => new { prodDb, aliExpProd });
                    foreach (var pair in pairs)
                    {
                        pair.aliExpProd.SkuCode = pair.prodDb.Sku;
                    }
                }
            }
            intersectProducts = products.Where(x => x.SkuCode != null).ToList();
            exceptProducts = products.Where(prod => productsInDb.All(prodDb => prodDb.AliExpressProductId != prod.ProductId)).ToList();
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

        public void ProcessUpdateDatabaseAliExpressProductId(IEnumerable<AliExpressProductDTO> aliExpressProducts)
        {
            if (aliExpressProducts.Any())
            {
                //в отдельный
                using (var connection = new SqlConnection(_configuration.GetConnectionString("SQLServerConnectionString")))
                {
                    connection.Open();
                    var productsInDb = connection.Query<Product>("select * from products where sku IN @skus", new { skus = aliExpressProducts.Select(x => x.SkuCode) });
                    var updateProducts = aliExpressProducts.Where(x => productsInDb.Any(t => t.Sku.Equals(x.SkuCode)
                        && (!t.AliExpressProductId.HasValue || t.AliExpressProductId != x.ProductId)
                    ));
                    if (updateProducts.Any())
                        UpdateAliExpressProductId(connection, updateProducts);
                }
            }
        }

        private void UpdateAliExpressProductId(SqlConnection connection, IEnumerable<AliExpressProductDTO> updateProducts)
        {
            foreach (var updateProduct in updateProducts)
            {
                connection.Execute(
                    "update products set aliExpressProductId = @aliExpressProductId, updatedAt = @updatedAt where sku = @sku",
                    new
                    {
                        aliExpressProductId = updateProduct.ProductId,
                        updatedAt = DateTimeOffset.Now.ToString("yyyy-MM-dd'T'HH:mm:ssK"),
                        sku = updateProduct.SkuCode
                    });
            }
        }

        public AliExpressProductDTO GetProduct(long productId)
        {
            ITopClient client = new DefaultTopClient(_aliExpressOptions.HttpsEndPoint, _aliExpressOptions.AppKey, _aliExpressOptions.AppSecret, "Json");
            AliexpressSolutionProductInfoGetRequest req = new AliexpressSolutionProductInfoGetRequest
            {
                ProductId = productId
            };
            AliexpressSolutionProductInfoGetResponse rsp = client.Execute(req, _aliExpressOptions.AccessToken);
            return ProductStringToDTO(rsp.Body);
        }

        //todo А если придется читать не json -а XML - разбить на отдельный классы/command/Queryes!!!
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

        public void UpdatePriceProduct(List<long> productIds)
        {
            throw new NotImplementedException();
        }
    }
}
