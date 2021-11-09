using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Coravel.Invocable;
using Microsoft.Extensions.Logging;
using YapartMarket.Core.BL;
using YapartMarket.Core.DTO;
using YapartMarket.Core.Exceptions;
using YapartMarket.Core.Extensions;
using YapartMarket.Core.Models.Azure;

namespace YapartMarket.React.Invocables
{
    public class UpdateOrdersFromAliExpressInvocable : IInvocable
    {
        private readonly IAliExpressOrderService _aliExpressOrderService;
        private readonly IAliExpressOrderReceiptInfoService _aliExpressOrderReceiptInfoService;
        private readonly ILogger<UpdateOrdersFromAliExpressInvocable> _logger;
        private readonly IMapper _mapper;

        public UpdateOrdersFromAliExpressInvocable(
            IAliExpressOrderService aliExpressOrderService,
            IAliExpressOrderReceiptInfoService aliExpressOrderReceiptInfoService,
            ILogger<UpdateOrdersFromAliExpressInvocable> logger,
            IMapper mapper)
        {
            _aliExpressOrderService = aliExpressOrderService;
            _aliExpressOrderReceiptInfoService = aliExpressOrderReceiptInfoService;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task Invoke()
        {
            _logger.LogInformation("Запуск процедуры обновления заказов");
            var dateTimeNow = DateTime.UtcNow;
            try
            {
                var yesterdayDateTime = new DateTimeWithZone(dateTimeNow.AddDays(-1).StartOfDay(), TimeZoneInfo.FindSystemTimeZoneById("Russian Standard Time"));
                var tommorowDateTime = new DateTimeWithZone(dateTimeNow.AddDays(+1).EndOfDay(), TimeZoneInfo.FindSystemTimeZoneById("Russian Standard Time"));
                var ordersDTO = _aliExpressOrderService.QueryOrderDetail(yesterdayDateTime.LocalTime, tommorowDateTime.LocalTime); 
                if (ordersDTO.Any())
                {
                    _logger.LogInformation("Получены заказы");
                    Debug.WriteLine("Получены заказы");
                    var aliExpressOrders = _mapper.Map<List<AliExpressOrderDTO>, List<AliExpressOrder>>(ordersDTO);
                    _logger.LogInformation("Сохранение новых заказов");
                    await _aliExpressOrderService.AddOrders(aliExpressOrders);
                    if (aliExpressOrders.Any())
                    {
                        _logger.LogInformation("Запись адреса получателя");
                        foreach (var aliExpressOrder in aliExpressOrders)
                        {
                            var orderReceiptDto = _aliExpressOrderReceiptInfoService.GetReceiptInfo(aliExpressOrder.OrderId);
                            _logger.LogInformation($"OrderId : {aliExpressOrder.OrderId}");
                            await _aliExpressOrderReceiptInfoService.InsertOrderReceipt(aliExpressOrder.OrderId, orderReceiptDto);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                _logger.LogWarning(e.Message);
                throw new UpdateOrdersFromAliExpressException($"Message : {e.Message} \n StackTrace : {e.StackTrace}");
            }
        }
    }
}
