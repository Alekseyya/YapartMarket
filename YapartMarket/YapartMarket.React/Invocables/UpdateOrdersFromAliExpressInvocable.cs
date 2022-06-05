﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Coravel.Invocable;
using Microsoft.Extensions.Logging;
using YapartMarket.Core.BL;
using YapartMarket.Core.BL.AliExpress;
using YapartMarket.Core.DTO;
using YapartMarket.Core.DTO.AliExpress.OrderGetResponse;
using YapartMarket.Core.Exceptions;
using YapartMarket.Core.Extensions;
using YapartMarket.Core.Models.Azure;

namespace YapartMarket.React.Invocables
{
    public class UpdateOrdersFromAliExpressInvocable : IInvocable
    {
        private readonly IAliExpressOrderService _aliExpressOrderService;
        private readonly IAliExpressOrderReceiptInfoService _aliExpressOrderReceiptInfoService;
        private readonly IAliExpressLogisticRedefiningService _aliExpressLogisticRedefiningService;
        private readonly IAliExpressLogisticOrderDetailService _aliExpressLogisticOrderDetailService;
        private readonly IAliExpressOrderFullfilService _aliExpressOrderFullfilService;
        private readonly IAliExpressProductService _aliExpressProductService;
        private readonly IAliExpressCategoryService _aliExpressCategoryService;
        private readonly ILogisticServiceOrderService _logisticServiceOrderService;
        private readonly ILogisticWarehouseOrderService _logisticWarehouseOrderService;
        private readonly ILogger<UpdateOrdersFromAliExpressInvocable> _logger;
        private readonly IMapper _mapper;

        public UpdateOrdersFromAliExpressInvocable(
            IAliExpressOrderService aliExpressOrderService,
            IAliExpressOrderReceiptInfoService aliExpressOrderReceiptInfoService,
            IAliExpressLogisticRedefiningService aliExpressLogisticRedefiningService,
            IAliExpressLogisticOrderDetailService aliExpressLogisticOrderDetailService,
            IAliExpressOrderFullfilService aliExpressOrderFullfilService,
            IAliExpressProductService aliExpressProductService,
            IAliExpressCategoryService aliExpressCategoryService,
            ILogisticServiceOrderService logisticServiceOrderService,
            ILogisticWarehouseOrderService logisticWarehouseOrderService,
            ILogger<UpdateOrdersFromAliExpressInvocable> logger,
            IMapper mapper)
        {
            _aliExpressOrderService = aliExpressOrderService;
            _aliExpressOrderReceiptInfoService = aliExpressOrderReceiptInfoService;
            _aliExpressLogisticRedefiningService = aliExpressLogisticRedefiningService;
            _aliExpressLogisticOrderDetailService = aliExpressLogisticOrderDetailService;
            _aliExpressOrderFullfilService = aliExpressOrderFullfilService;
            _aliExpressProductService = aliExpressProductService;
            _aliExpressCategoryService = aliExpressCategoryService;
            _logisticServiceOrderService = logisticServiceOrderService;
            _logisticWarehouseOrderService = logisticWarehouseOrderService;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task Invoke()
        {
            _logger.LogInformation("Запуск процедуры обновления заказов");
            var dateTimeNow = DateTime.UtcNow;
            try
            {
                var ordersDTO = await _aliExpressOrderService.QueryOrderDetail(dateTimeNow.AddDays(-1).StartOfDay(), dateTimeNow.AddDays(+1).EndOfDay()); 
                if (ordersDTO.IsAny())
                {
                    _logger.LogInformation("Получены заказы");
                    Debug.WriteLine("Получены заказы");
                    var aliExpressOrders = _mapper.Map<List<OrderDto>, List<AliExpressOrder>>(ordersDTO);
                    _logger.LogInformation("Сохранение новых заказов");
                    await _aliExpressOrderService.AddOrders(aliExpressOrders);
                    if (aliExpressOrders.Any())
                    {
                        _logger.LogInformation("Запись адреса получателя");
                        foreach (var aliExpressOrder in aliExpressOrders)
                        {
                            var orderReceiptDto = await _aliExpressOrderReceiptInfoService.GetReceiptInfo(aliExpressOrder.OrderId);
                            _logger.LogInformation($"OrderId : {aliExpressOrder.OrderId}");
                            await _aliExpressOrderReceiptInfoService.InsertOrderReceipt(aliExpressOrder.OrderId, orderReceiptDto);

                            _logger.LogInformation($"Получение логистического номера заказа {aliExpressOrder.OrderId}");
                            var logisticOrderDetail = _aliExpressLogisticOrderDetailService.GetLogisticOrderDetailRequest(aliExpressOrder.OrderId);
                            await _aliExpressLogisticOrderDetailService.ProcessLogisticsOrderDetailAsync(logisticOrderDetail);
                            var orderDetails = aliExpressOrder.AliExpressOrderDetails;
                            foreach (var orderDetail in orderDetails)
                            {
                                await _aliExpressProductService.ProcessUpdateProduct(orderDetail.ProductId);
                                await _aliExpressCategoryService.UpdateCategoryByProductId(orderDetail.ProductId);
                            }
                            //_logger.LogInformation("Подтверждение заказа.");
                            //await _logisticWarehouseOrderService.CreateOrderAsync(aliExpressOrder.OrderId);
                            //await _logisticWarehouseOrderService.CreateWarehouseOrderAsync(aliExpressOrder.OrderId);
                            //await _logisticWarehouseOrderService.CreateWarehouseAsync(aliExpressOrder.OrderId);
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
