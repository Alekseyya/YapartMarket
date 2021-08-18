using System;
using System.Collections.Generic;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Top.Api;
using Top.Api.Request;
using Top.Api.Response;
using YapartMarket.Core.BL;
using YapartMarket.Core.Config;
using YapartMarket.Core.DTO;

namespace YapartMarket.BL.Implementation
{
    public class AliExpressProductService : IAliExpressProductService
    {
        private readonly AliExpressOptions _aliExpressOptions;

        public AliExpressProductService(IOptions<AliExpressOptions> options)
        {
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

        public string GetProducts()
        {
            ITopClient client = new DefaultTopClient(_aliExpressOptions.HttpsEndPoint, _aliExpressOptions.AppKey, _aliExpressOptions.AppSecret, "Json");
            var req = new AliexpressSolutionProductListGetRequest();
            var obj1 = new AliexpressSolutionProductListGetRequest.ItemListQueryDomain
            {
                ProductStatusType = "onSelling"
            };
            req.AeopAEProductListQuery_ = obj1;
            var rsp = client.Execute(req, _aliExpressOptions.AccessToken);
            return rsp.Body;
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
