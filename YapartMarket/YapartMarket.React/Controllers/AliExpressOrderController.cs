﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using YapartMarket.Core.BL;
using YapartMarket.Core.BL.AliExpress;
using YapartMarket.Core.Extensions;
using YapartMarket.Core.Models.Azure;
using YapartMarket.React.Invocables;
using YapartMarket.React.ViewModels;

namespace YapartMarket.React.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AliExpressOrderController : Controller
    {
        private readonly IAliExpressOrderService _aliExpressOrderService;
        private readonly IAliExpressOrderReceiptInfoService _aliExpressOrderReceiptInfoService;
        private readonly IAliExpressLogisticRedefiningService _aliExpressLogisticRedefiningService;
        private readonly IAliExpressLogisticOrderDetailService _aliExpressLogisticOrderDetailService;
        private readonly IAliExpressProductService _aliExpressProductService;
        private readonly IAliExpressCategoryService _aliExpressCategoryService;
        private readonly IAliExpressOrderFullfilService _aliExpressOrderFullfilService;
        private readonly ILogisticServiceOrderService _logisticServiceOrderService;
        private readonly ILogisticWarehouseOrderService _logisticWarehouseOrderService;
        private readonly ILogger<UpdateOrdersFromAliExpressInvocable> _logger;
        private readonly IMapper _mapper;

        public AliExpressOrderController(IAliExpressOrderService aliExpressOrderService,
            IAliExpressOrderReceiptInfoService aliExpressOrderReceiptInfoService,
            IAliExpressLogisticRedefiningService aliExpressLogisticRedefiningService,
            IAliExpressLogisticOrderDetailService aliExpressLogisticOrderDetailService,
            IAliExpressProductService aliExpressProductService,
            IAliExpressCategoryService aliExpressCategoryService,
            IAliExpressOrderFullfilService aliExpressOrderFullfilService,
            ILogisticServiceOrderService logisticServiceOrderService,
            ILogisticWarehouseOrderService logisticWarehouseOrderService,
            ILogger<UpdateOrdersFromAliExpressInvocable> logger,
            IMapper mapper)
        {
            _aliExpressOrderService = aliExpressOrderService;
            _aliExpressOrderReceiptInfoService = aliExpressOrderReceiptInfoService;
            _aliExpressLogisticRedefiningService = aliExpressLogisticRedefiningService;
            _aliExpressLogisticOrderDetailService = aliExpressLogisticOrderDetailService;
            _aliExpressProductService = aliExpressProductService;
            _aliExpressCategoryService = aliExpressCategoryService;
            _aliExpressOrderFullfilService = aliExpressOrderFullfilService;
            _logisticServiceOrderService = logisticServiceOrderService;
            _logisticWarehouseOrderService = logisticWarehouseOrderService;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpGet]
        [Route("CurrentDay")]
        [Produces("application/json")]
        public async Task<IActionResult> Get()
        {
            try
            {
                var ordersByDay = await _aliExpressOrderService.GetOrders(DateTime.Now.AddDays(-1).StartOfDay(), DateTime.Now.EndOfDay());
                if (ordersByDay.IsAny())
                    return Ok(_mapper.Map<IEnumerable<AliExpressOrder>, IEnumerable<AliExpressOrderViewModel>>(ordersByDay));
                return Ok();
            }
            catch (Exception e)
            {

                return BadRequest(e.Message);
            }
           
        }
        [HttpGet]
        [Route("Yesterday")]
        [Produces("application/json")]
        public async Task<IActionResult> Yesterday()
        {
            try
            {
                var ordersByDay = await _aliExpressOrderService.GetOrders(DateTime.Now.AddDays(-2).StartOfDay(), DateTime.Now.AddDays(-1).EndOfDay());
                if (ordersByDay.IsAny())
                    return Ok(_mapper.Map<IEnumerable<AliExpressOrder>, IEnumerable<AliExpressOrderViewModel>>(ordersByDay));
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }

        }

        [HttpGet]
        [Route("Day")]
        [Produces("application/json")]
        public async Task<IActionResult> Get(DateTime day)
        {
            var ordersByDay = await _aliExpressOrderService.GetOrders(day.StartOfDay(), day.EndOfDay());
            if (ordersByDay.IsAny())
                return Ok(_mapper.Map<IEnumerable<AliExpressOrder>, IEnumerable<AliExpressOrderViewModel>>(ordersByDay));
            return Ok();
        }

        [HttpGet]
        [Route("updateOrders")]
        [Produces("application/json")]
        public async Task<IActionResult> UpdateOrders()
        {
            try
            {
                var updateOrders = new UpdateOrdersFromAliExpressInvocable(_aliExpressOrderService,
                    _aliExpressOrderReceiptInfoService, _aliExpressLogisticRedefiningService,
                    _aliExpressLogisticOrderDetailService, _aliExpressOrderFullfilService,
                    _aliExpressProductService,
                    _aliExpressCategoryService,
                    _logisticServiceOrderService,
                    _logisticWarehouseOrderService,
                    _logger, _mapper);
                await updateOrders.Invoke();
            }
            catch (Exception e)
            {
                return BadRequest($"Error {e.Message} \n Stacktrace {e.StackTrace}");
            }
            
            return Ok();
        }

        [HttpPost]
        [Route("downloadNewOrders")]
        public async Task<IActionResult> DownloadNewOrders()
        {
            try
            {
                var aliExpressOrders = await _aliExpressOrderService.QueryOrderDetail(DateTime.Now.AddDays(-8).StartOfDay(), DateTime.Now.AddDays(+1).EndOfDay());
                await _aliExpressOrderService.AddOrders(aliExpressOrders.ToList());
            }
            catch (Exception e)
            {
                return BadRequest(e.Message + "\n" + e.StackTrace);
            }
            return StatusCode(200);
        }
    }
}
