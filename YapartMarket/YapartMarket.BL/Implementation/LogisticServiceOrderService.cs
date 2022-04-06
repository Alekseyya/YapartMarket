using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
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
    public sealed class LogisticServiceOrderService : ILogisticServiceOrderService
    {
        private readonly IOptions<AliExpressOptions> _options;
        private readonly IMapper _mapper;
        private readonly ILogisticServiceOrderRepository _logisticServiceOrderRepository;
        private readonly ITopClient _client;

        public LogisticServiceOrderService(ILogger<LogisticServiceOrderService> logger,
            IOptions<AliExpressOptions> options, IMapper mapper, ILogisticServiceOrderRepository logisticServiceOrderRepository)
        {
            _options = options;
            _mapper = mapper;
            _logisticServiceOrderRepository = logisticServiceOrderRepository;
            _client = new DefaultTopClient(options.Value.HttpsEndPoint, options.Value.AppKey, options.Value.AppSecret, "Json");
        }

        public List<LogisticsServiceOrderResultDTO> GetLogisticServiceOrderRequest(long orderId)
        {
            var req = new AliexpressLogisticsRedefiningGetonlinelogisticsservicelistbyorderidRequest();
            req.OrderId = orderId;
            req.Locale = "ru_RU";
            var rsp = _client.Execute(req, _options.Value.AccessToken);
            var aliExpressOrderDetailDTO = JsonConvert.DeserializeObject<LogisticsServiceOrderRootDTO>(rsp.Body);
            return aliExpressOrderDetailDTO.LogisticsServiceOrderDto.LogisticsServiceOrderResultListDto.LogisticsServiceOrderResultDtos;
        }

        public async Task ProcessLogisticServiceOrderAsync(long orderId, List<LogisticsServiceOrderResultDTO> logisticsServiceOrderResultDtos)
        {
            try
            {
                if (!logisticsServiceOrderResultDtos.Any())
                    return; ;
                var logisticServiceOrderDto = logisticsServiceOrderResultDtos.FirstOrDefault();
                var logisticOrderService = _mapper.Map<LogisticsServiceOrderResultDTO, LogisticServiceOrder>(logisticServiceOrderDto);
                logisticOrderService.OrderId = orderId;
                var logisticOrderServiceDb = await _logisticServiceOrderRepository.GetAsync(
                    "select * from dbo.logistic_service_order where order_id = @order_id", new { order_id = orderId });
                if (!logisticOrderServiceDb.IsAny())
                {
                    var insertOrder = new LogisticServiceOrder().InsertString("dbo.logistic_service_order");
                    await _logisticServiceOrderRepository.InsertAsync(insertOrder, new
                    {
                        order_id = logisticOrderService.OrderId,
                        warehouse_name = logisticOrderService.WarehouseName,
                        logistics_service_name = logisticOrderService.LogisticsServiceName,
                        logistics_service_id = logisticOrderService.LogisticsServiceId,
                        delivery_address = logisticOrderService.DeliveryAddress
                    });
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            
        }
        public async Task<LogisticServiceOrder> GetLogisticServiceOrder(long orderId)
        {
            var logisticServiceOrder = await _logisticServiceOrderRepository.GetAsync("select * from dbo.logistic_service_order where order_id = @order_id", new { order_id = orderId });
            return logisticServiceOrder.FirstOrDefault();
        }
    }
}
