using System;
using System.Collections.Generic;
using System.Net;
using System.Runtime.CompilerServices;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Top.Api;
using Top.Api.Request;
using YapartMarket.Core.BL;
using YapartMarket.Core.Config;
using YapartMarket.Core.DTO;
using YapartMarket.Core.JsonConverters;

[assembly: InternalsVisibleTo("UnitTests")]
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
        public List<AliExpressOrderListDTO> QueryOrderDetail(DateTime? startDateTime = null, DateTime? endDateTime = null)
        {
            try
            {
                ITopClient client = new DefaultTopClient(_aliExpressOptions.HttpsEndPoint, _aliExpressOptions.AppKey, _aliExpressOptions.AppSecret, "Json");
                AliexpressSolutionOrderGetRequest req = new AliexpressSolutionOrderGetRequest();
                AliexpressSolutionOrderGetRequest.OrderQueryDomain obj1 = new AliexpressSolutionOrderGetRequest.OrderQueryDomain();
                obj1.CreateDateEnd = endDateTime?.ToString("yyy-MM-dd hh:mm:ss");
                obj1.CreateDateStart = startDateTime?.ToString("yyy-MM-dd hh:mm:ss");
                //obj1.OrderStatusList = new List<string>{ OrderStatus.SELLER_PART_SEND_GOODS.ToString() };
                obj1.PageSize = 20;
                obj1.CurrentPage = 1L; //todo получить все заказы, цикл на количество страниц
                req.Param0_ = obj1;
                var rsp = client.Execute(req, _aliExpressOptions.AccessToken);
                return DeserializeAliExpressOrderList(rsp.Body);
            }
            catch (WebException e)
            {
                throw;
            }

        }

        public List<AliExpressOrderListDTO> DeserializeAliExpressOrderList(string json)
        {
            var jsonObject = JObject.Parse(json);
            var orderJson = jsonObject.SelectToken("aliexpress_solution_order_get_response.result.target_list.order_dto").ToString();
            return JsonConvert.DeserializeObject<List<AliExpressOrderListDTO>>(orderJson, new AliExpressOrderDetailConverter());
        }
    }
}
