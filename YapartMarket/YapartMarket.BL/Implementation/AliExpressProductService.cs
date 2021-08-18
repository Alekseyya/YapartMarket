using System;
using System.Collections.Generic;
using System.Linq;
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
        public void UpdateInventoryProducts(List<long> productIds)
        {
            ITopClient client = new DefaultTopClient(_aliExpressOptions.HttpsEndPoint, _aliExpressOptions.AppKey, _aliExpressOptions.AppSecret);
            AliexpressSolutionBatchProductInventoryUpdateRequest req = new AliexpressSolutionBatchProductInventoryUpdateRequest();
            List<AliexpressSolutionBatchProductInventoryUpdateRequest.SynchronizeProductRequestDtoDomain> list2 = new List<AliexpressSolutionBatchProductInventoryUpdateRequest.SynchronizeProductRequestDtoDomain>();
            AliexpressSolutionBatchProductInventoryUpdateRequest.SynchronizeProductRequestDtoDomain obj3 = new AliexpressSolutionBatchProductInventoryUpdateRequest.SynchronizeProductRequestDtoDomain();
            list2.Add(obj3);
            obj3.ProductId = 1000005237852L;
            List<AliexpressSolutionBatchProductInventoryUpdateRequest.SynchronizeSkuRequestDtoDomain> list5 = new List<AliexpressSolutionBatchProductInventoryUpdateRequest.SynchronizeSkuRequestDtoDomain>();
            AliexpressSolutionBatchProductInventoryUpdateRequest.SynchronizeSkuRequestDtoDomain obj6 = new AliexpressSolutionBatchProductInventoryUpdateRequest.SynchronizeSkuRequestDtoDomain();
            list5.Add(obj6);
            obj6.SkuCode = "123abc";
            obj6.Inventory = 99L;
            obj3.MultipleSkuUpdateList = list5;
            req.MutipleProductUpdateList_ = list2;
            AliexpressSolutionBatchProductInventoryUpdateResponse rsp = client.Execute(req, _aliExpressOptions.AccessToken);
            Console.WriteLine(rsp.Body);
        }

        public IEnumerable<AliExpressProductDTO> GetProducts()
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
                        listProducts.AddRange(tmpListProductFromJson);
                    currentPage++;
                } while (haveElement);
                return listProducts.AsEnumerable();
            }
            catch (Exception)
            {
                return listProducts.AsEnumerable();
            }
        }

        public void ProcessAliExpressProductId(List<AliExpressProductDTO> aliExpressProducts)
        {
            using (var connection = new SqlConnection(_configuration.GetConnectionString("SQLServerConnectionString")))
            {
                connection.Open();
                var productsInDb = connection.Query<Product>("select * from products where sku IN @skus", new { skus = aliExpressProducts.Select(x => x.SkuCode) });
                var updateProducts = aliExpressProducts.Where(x => productsInDb.Any(t => t.Sku.Equals(x.SkuCode) 
                                            && (!t.AliExpressProductId.HasValue || t.AliExpressProductId != x.ProductId)
                                            ));
                if(updateProducts.Any())
                    UpdateAliExpressProductId(connection, updateProducts);
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

        public AliExpressProductDTO ProductStringToDTO(string json)
        {
            var jsonObject = JObject.Parse(json);
            var productJson = jsonObject.SelectToken("aliexpress_solution_product_info_get_response.result.aeop_ae_product_s_k_us.global_aeop_ae_product_sku")[0].ToString();
            try
            {
                if (!string.IsNullOrEmpty(productJson))
                {
                    var aliExpressProduct = JsonConvert.DeserializeObject<AliExpressProductDTO>(productJson);
                    var productId = (long) jsonObject.SelectToken("aliexpress_solution_product_info_get_response.result.product_id");
                    var description = jsonObject.SelectToken("aliexpress_solution_product_info_get_response.result.subject").ToString();
                    if (aliExpressProduct != null)
                    {
                        aliExpressProduct.ProductId = productId;
                        aliExpressProduct.Description = description;
                        return aliExpressProduct;
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            return null;
        }

        public void UpdatePriceProduct(List<long> productIds)
        {
            throw new NotImplementedException();
        }
    }
}
