﻿using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using YapartMarket.Core.BL;
using YapartMarket.Core.Extensions;
using YapartMarket.Core.Models.Azure;
using YapartMarket.WebApi.Model.AliExpress;
using YapartMarket.WebApi.Services;

namespace YapartMarket.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AliExpressController : ControllerBase
    {
        private readonly IAliExpressOrderService _aliExpressOrderService;
        private readonly IAliExpressProductService _productService;
        private readonly IMapper _mapper;

        public AliExpressController(IAliExpressOrderService aliExpressOrderService, IAliExpressProductService productService, IMapper mapper)
        {
            _aliExpressOrderService = aliExpressOrderService;
            _productService = productService;
            _mapper = mapper;
        }
        [HttpGet]
        [Route("CurrentDay")]
        [Produces("application/json")]
        public async Task<IActionResult> Get()
        {
            try
            {
                var ordersByDay = await _aliExpressOrderService.QueryOrderDetail(DateTime.Now.AddDays(-1).StartOfDay(), DateTime.Now.EndOfDay());
                var orderService = new OrderService();
                if (ordersByDay.IsAny())
                    return Ok(orderService.Convert(ordersByDay));
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
                    return Ok(_mapper.Map<IEnumerable<AliExpressOrder>, IEnumerable<Order>>(ordersByDay));
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
                return Ok(_mapper.Map<IEnumerable<AliExpressOrder>, IEnumerable<Order>>(ordersByDay));
            return Ok();
        }
        [HttpGet]
        [Route("updateStocks")]
        [Produces("application/json")]
        public async Task<IActionResult> UpdateStocks()
        {
            var response = await _productService.ProcessUpdateStocks();
            return Ok(response);
        }

        [HttpGet]
        [Route("updateProducts")]
        [Produces("application/json")]
        public async Task<IActionResult> UpdateProducts()
        {
            await _productService.ProcessUpdateProductSku();
            return Ok();
        }

        [HttpGet]
        [Route("createLogisticOrder")]
        [Produces("application/json")]
        public async Task<IActionResult> CreateLogisticOrder()
        {
            await _aliExpressOrderService.CreateLogisticOrderAsync();
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
