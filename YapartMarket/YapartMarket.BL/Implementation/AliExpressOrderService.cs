using System;
using System.Collections.Generic;
using System.Net;
using System.Runtime.CompilerServices;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Top.Api;
using Top.Api.Request;
using YapartMarket.Core.BL;
using YapartMarket.Core.Config;
using YapartMarket.Core.DateStructures;
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
        
        public List<AliExpressOrderListDTO> QueryOrderDetail(DateTime? startDateTime = null, DateTime? endDateTime = null, List<OrderStatus> orderStatusList = null)
        {
            var currentPage = 1;
            ITopClient client = new DefaultTopClient(_aliExpressOptions.HttpsEndPoint, _aliExpressOptions.AppKey, _aliExpressOptions.AppSecret, "Json");
            var aliExpressOrderList = new List<AliExpressOrderListDTO>();
            do
            {
                try
                {
                    AliexpressSolutionOrderGetRequest req = new AliexpressSolutionOrderGetRequest();
                    AliexpressSolutionOrderGetRequest.OrderQueryDomain obj1 = new AliexpressSolutionOrderGetRequest.OrderQueryDomain();
                    obj1.CreateDateEnd = endDateTime?.ToString("yyy-MM-dd hh:mm:ss");
                    obj1.CreateDateStart = startDateTime?.ToString("yyy-MM-dd hh:mm:ss");
                    if(orderStatusList != null)
                        obj1.OrderStatusList = new List<string>{ OrderStatus.SELLER_PART_SEND_GOODS.ToString() };
                    obj1.PageSize = 20;
                    obj1.CurrentPage = currentPage;
                    req.Param0_ = obj1;
                    var rsp = client.Execute(req, _aliExpressOptions.AccessToken);
                    var deserializeAliExpressOrderList = DeserializeAliExpressOrderList(rsp.Body);
                    if(deserializeAliExpressOrderList.Any())
                        aliExpressOrderList.AddRange(deserializeAliExpressOrderList);
                    else
                        break;
                } //todo проверка сообщения
                catch (Exception e)
                {
                    break;
                }
                currentPage++;
            } while (true);
            return aliExpressOrderList;
        }

        public List<AliExpressOrderListDTO> DeserializeAliExpressOrderList(string json)
        {
            var aliExpressResponseResult = JsonConvert.DeserializeObject<AliExpressGetOrderRoot>(json)?.AliExpressSolutionOrderGetResponseDTO.AliExpressSolutionOrderGetResponseResultDto;
            if (!aliExpressResponseResult.AliExpressOrderListDTOs.Any() && aliExpressResponseResult.TotalCount == 0)
            {
                return null;
            }
            return JsonConvert.DeserializeObject<AliExpressGetOrderRoot>(json)?.AliExpressSolutionOrderGetResponseDTO.AliExpressSolutionOrderGetResponseResultDto.AliExpressOrderListDTOs;
        }
    }
}
