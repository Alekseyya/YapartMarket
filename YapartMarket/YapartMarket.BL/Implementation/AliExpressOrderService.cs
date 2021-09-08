using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Top.Api;
using Top.Api.Request;
using Top.Api.Response;
using YapartMarket.Core.BL;
using YapartMarket.Core.Config;
using YapartMarket.Core.DateStructures;

namespace YapartMarket.BL.Implementation
{
    public class AliExpressOrderService : IAliExpressOrderService
    {
        private readonly ILogger<AliExpressOrderService> _logger;
        private readonly AliExpressOptions _aliExpressOptions;

        public AliExpressOrderService(ILogger<AliExpressOrderService> logger, IOptions<AliExpressOptions> options)
        {
            _logger = logger;
            _aliExpressOptions = options.Value;
        }
        //проверить на количество закалов не влезающих на одну страницу
        public void QueryOrderDetail(DateTime startDateTime = default, DateTime endDateTime= default)
        {
            ITopClient client = new DefaultTopClient(_aliExpressOptions.HttpsEndPoint, _aliExpressOptions.AppKey, _aliExpressOptions.AppSecret, "Json");
            
            AliexpressSolutionOrderGetRequest req = new AliexpressSolutionOrderGetRequest();
            AliexpressSolutionOrderGetRequest.OrderQueryDomain obj1 = new AliexpressSolutionOrderGetRequest.OrderQueryDomain();
            obj1.CreateDateEnd = endDateTime.ToString("yyy-MM-dd hh:mm:ss");
            obj1.CreateDateStart = startDateTime.ToString("yyy-MM-dd hh:mm:ss");
            obj1.ModifiedDateStart = "2017-10-12 12:12:12";
            obj1.OrderStatusList = new List<string>{ OrderStatus.SELLER_PART_SEND_GOODS.ToString() };
            obj1.PageSize = 20;
            obj1.CurrentPage = 1L;
            req.Param0_ = obj1;
            AliexpressSolutionOrderGetResponse rsp = client.Execute(req, _aliExpressOptions.AccessToken);
        }
    }
}
