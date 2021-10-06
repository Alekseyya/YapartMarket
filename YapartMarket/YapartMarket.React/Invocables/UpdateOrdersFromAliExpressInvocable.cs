using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Coravel.Invocable;
using Microsoft.Extensions.Logging;
using YapartMarket.Core.BL;
using YapartMarket.Core.DTO;
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
            var ordersDTO = _aliExpressOrderService.QueryOrderDetail(dateTimeNow.AddDays(-1).StartOfDay(), dateTimeNow.AddDays(+1).EndOfDay());
            _logger.LogInformation("Получены заказы");
            var aliExpressOrders = _mapper.Map<List<AliExpressOrderDTO>, List<AliExpressOrder>>(ordersDTO);
            _logger.LogInformation("Сохранение новых заказов");
            await _aliExpressOrderService.AddOrders(aliExpressOrders);
            _logger.LogInformation("Запись адреса получателя");
            foreach (var aliExpressOrder in aliExpressOrders)
            {
                var orderReceiptDto = _aliExpressOrderReceiptInfoService.GetReceiptInfo(aliExpressOrder.OrderId);
                _logger.LogInformation($"OrderId : {aliExpressOrder.OrderId}");
                await _aliExpressOrderReceiptInfoService.InsertOrderReceipt(orderReceiptDto);
            }
        }
    }
}
