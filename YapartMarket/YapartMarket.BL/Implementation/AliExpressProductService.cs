using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Top.Api;
using Top.Api.Request;
using Top.Api.Response;
using YapartMarket.Core.BL;
using YapartMarket.Core.Config;
using YapartMarket.Core.DTO;
using YapartMarket.Core.Extensions;
using YapartMarket.Core.Models.Azure;

namespace YapartMarket.BL.Implementation
{
    public class AliExpressProductService : IAliExpressProductService
    {
        private readonly IConfiguration _configuration;
        private readonly AliExpressOptions _aliExpressOptions;

        public AliExpressProductService(IOptions<AliExpressOptions> options, IConfiguration configuration)
        {
            _configuration = configuration;
            _aliExpressOptions = options.Value;
        }
        public void UpdateInventoryProducts(List<AliExpressProductDTO> aliExpressProducts)
        {
            try
            {
                ITopClient client = new DefaultTopClient(_aliExpressOptions.HttpsEndPoint, _aliExpressOptions.AppKey, _aliExpressOptions.AppSecret, "Json");
                AliexpressSolutionBatchProductInventoryUpdateRequest req = new AliexpressSolutionBatchProductInventoryUpdateRequest();
                for (int i = 0; i < aliExpressProducts.Count; i += 20)
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

        public IEnumerable<AliExpressProductDTO> GetProductsAliExpress(Expression<Func<AliExpressProductDTO, bool>> conditionFunction = null)
        {
            var listProducts = new List<AliExpressProductDTO>();
            bool haveElement = true;
            long currentPage = 1;
            try
            {
                do
                {
                    ITopClient client = new DefaultTopClient(_aliExpressOptions.HttpsEndPoint, _aliExpressOptions.AppKey, _aliExpressOptions.AppSecret, "Json");
                    var req = new AliexpressSolutionProductListGetRequest();
                    var obj1 = new AliexpressSolutionProductListGetRequest.ItemListQueryDomain
                    {
                        CurrentPage = currentPage,
                        ProductStatusType = "onSelling",
                        PageSize = 99
                    };
                    req.AeopAEProductListQuery_ = obj1;
                    var rsp = client.Execute(req, _aliExpressOptions.AccessToken);
                    var tmpListProductFromJson = GetProductFromJson(rsp.Body);
                    if (!tmpListProductFromJson.Any())
                        haveElement = false;
                    else
                    {
                        //обновление коллекции полем SKUCode
                        tmpListProductFromJson = UpdateSkuFromAliExpress(tmpListProductFromJson, client);
                        tmpListProductFromJson = SetInventoryFromDatabase(tmpListProductFromJson.ToList());
                        listProducts.AddRange(tmpListProductFromJson);
                    }
                    currentPage++;
                } while (haveElement);

                if (conditionFunction != null)
                    return listProducts.AsQueryable().Where(conditionFunction).AsEnumerable();
                return listProducts.AsEnumerable(); 
            }
            catch (Exception)
            {
                if (conditionFunction != null)
                   return listProducts.AsQueryable().Where(conditionFunction).AsEnumerable();
                return listProducts.AsEnumerable();
            }
        }

        private IEnumerable<T> UpdateSkuFromAliExpress<T>(IEnumerable<T> products, ITopClient client) where T : AliExpressProductDTO
        {
            if (products.Any())
            {
                ReturnProductNotFoundDatabase(products, out IEnumerable<T> intersectProducts, out IEnumerable<T> exceptProducts);
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
                using (var connection = new SqlConnection(_configuration.GetConnectionString("SQLServerConnectionString")))
                {
                    connection.Open();
                    productsInDb = connection.Query<Product>("select * from products where aliExpressProductId IN @aliExpressProductIds", new { aliExpressProductIds = products.Select(x => x.ProductId) });
                }
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
