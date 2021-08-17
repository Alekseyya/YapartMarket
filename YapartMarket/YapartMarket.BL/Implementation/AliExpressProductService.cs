using System;
using System.Collections.Generic;
using Microsoft.Extensions.Options;
using Top.Api;
using Top.Api.Request;
using Top.Api.Response;
using YapartMarket.Core.BL;
using YapartMarket.Core.Config;

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

        public string GetProduct(long productId)
        {
            ITopClient client = new DefaultTopClient(_aliExpressOptions.HttpsEndPoint, _aliExpressOptions.AppKey, _aliExpressOptions.AppSecret, "Json");
            AliexpressSolutionProductInfoGetRequest req = new AliexpressSolutionProductInfoGetRequest
            {
                ProductId = productId
            };
            AliexpressSolutionProductInfoGetResponse rsp = client.Execute(req, _aliExpressOptions.AccessToken);
            return rsp.Body;
        }

        public void UpdatePriceProduct(List<long> productIds)
        {
            throw new NotImplementedException();
        }
    }
}
