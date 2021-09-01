using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
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
        public void UpdateInventoryProducts(IEnumerable<Product> products)
        {
            try
            {
                ITopClient client = new DefaultTopClient(_aliExpressOptions.HttpsEndPoint, _aliExpressOptions.AppKey, _aliExpressOptions.AppSecret, "Json");
                AliexpressSolutionBatchProductInventoryUpdateRequest req = new AliexpressSolutionBatchProductInventoryUpdateRequest();
                for (int i = 0; i < products.Count(); i += 20)
                {
                    var takeProducts = products.Skip(i).Take(20);
                    if (takeProducts.Any())
                    {
                        var reqMultipleProductUpdateList = new List<AliexpressSolutionBatchProductInventoryUpdateRequest.SynchronizeProductRequestDtoDomain>();
                        foreach (var takeProduct in takeProducts)
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
                        await AddNewProducts(listProducts);
                    }
                    currentPage++;
                } while (haveElement);
                await ProcessUpdateSku();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task ProcessUpdateSku()
        {
            bool repeat = false;
            do
            {
                var productsInDbEmptySku = await _azureAliExpressProductRepository.GetAsync("select * from aliExpressProducts where sku is null");
                //обновить у них SKU
                if (productsInDbEmptySku.IsAny())
                {
                    ITopClient getClient = new DefaultTopClient(_aliExpressOptions.HttpsEndPoint,
                        _aliExpressOptions.AppKey, _aliExpressOptions.AppSecret, "Json");
                    var updatedProductsFromDb = new List<AliExpressProduct>();
                    try
                    {
                        var countUpdateData = 0;
                        var skipRows = 0;
                        for (int i = 0; i < productsInDbEmptySku.Count(); i++)
                        {
                            var productInDb = productsInDbEmptySku.ElementAt(i);
                            var requestProductInfo = new AliexpressSolutionProductInfoGetRequest
                            {
                                ProductId = productInDb.ProductId
                            };
                            var productInfoResponse = getClient.Execute(requestProductInfo, _aliExpressOptions.AccessToken);
                            var productInfo = ProductStringToDTO(productInfoResponse.Body); //read Json
                            if (productInfo != null)
                            {
                                productInDb.SKU = productInfo.SkuCode;
                                productInDb.Inventory = productInfo.SkuStock;
                            }

                            countUpdateData++;
                            updatedProductsFromDb.Add(productInDb);
                            if (countUpdateData == 100 || i == productsInDbEmptySku.Count() - 1)
                            {
                                updatedProductsFromDb = new List<AliExpressProduct>();
                                foreach (var product in productsInDbEmptySku.Skip(skipRows).Take(countUpdateData))
                                {
                                    await _azureAliExpressProductRepository
                                        .Update( //todo переписать на формирование большого update text!!!!
                                            "update aliExpressProducts set sku = @sku, inventory = @inventory, updatedAt = @updatedAt where productId = @productId",
                                            new
                                            {
                                                sku = product.SKU,
                                                inventory = product.Inventory,
                                                updatedAt = DateTimeOffset.Now.ToString("yyyy-MM-dd'T'HH:mm:ssK"),
                                                productId = product.ProductId
                                            });
                                }

                                skipRows += countUpdateData;
                                countUpdateData = 0;
                            }
                        }
                    }
                    catch (WebException ex)
                    {
                        if (ex.Status == WebExceptionStatus.Timeout)
                        {
                            if (updatedProductsFromDb.Any())
                            {
                                foreach (var updateProductFromDb in updatedProductsFromDb)
                                {
                                    await _azureAliExpressProductRepository.Update(
                                        "update aliExpressProducts set sku = @sku, inventory = @inventory, updatedAt = @updatedAt where productId = @productId",
                                        new
                                        {
                                            sku = updateProductFromDb.SKU,
                                            inventory = updateProductFromDb.Inventory,
                                            updatedAt = DateTimeOffset.Now.ToString("yyyy-MM-dd'T'HH:mm:ssK"),
                                            productId = updateProductFromDb.ProductId
                                        });
                                }
                            }

                            await Task.Delay(10000);
                            repeat = true;
                        }
                    }
                }
                else
                    repeat = false;
            } while (repeat);
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
                    }, splitOn: "sku");
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
               await UpdateAliExpressProductId(updateProducts);

        }

        public async Task<IEnumerable<Product>> GetProductWhereAliExpressProductIdIsNull()
        {
            using (var connection = new SqlConnection(_configuration.GetConnectionString("SQLServerConnectionString")))
            {
                await connection.OpenAsync();
                _logger.LogInformation("Связывание таблиц AliExpressProduct с Product");
                var productsInDb = await connection.QueryAsync<Product, AliExpressProduct, Product>(
                    "select * FROM dbo.products p inner join dbo.aliExpressProducts aep on p.sku = aep.sku WHERE p.aliExpressProductId is NULL",
                    (product, aliExpressProduct) =>
                    {
                        product.AliExpressProduct = aliExpressProduct;
                        product.AliExpressProductId = aliExpressProduct.ProductId;
                        return product;
                    }, splitOn: "sku");
                var result = productsInDb.GroupBy(x => x.Sku).Select(y => y.First());
                _logger.LogInformation($"Количество записей, которые стоит обновить {result.Count()}");
                return result;
            }
        }

        private async Task UpdateAliExpressProductId(IEnumerable<Product> updateProducts)
        {
            using (var connection = new SqlConnection(_configuration.GetConnectionString("SQLServerConnectionString")))
            {
                await connection.OpenAsync();
                foreach (var updateProduct in updateProducts)
                {
                    await connection.ExecuteAsync(
                        "update products set aliExpressProductId = @aliExpressProductId, updatedAt = @updatedAt where sku = @sku",
                        new
                        {
                            aliExpressProductId = updateProduct.AliExpressProductId,
                            updatedAt = DateTimeOffset.Now.ToString("yyyy-MM-dd'T'HH:mm:ssK"),
                            sku = updateProduct.Sku
                        });
                }
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
