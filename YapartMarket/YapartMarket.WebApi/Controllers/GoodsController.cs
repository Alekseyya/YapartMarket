﻿using Microsoft.AspNetCore.Mvc;
using YapartMarket.Core.Extensions;
using YapartMarket.WebApi.Services.Interfaces;
using YapartMarket.WebApi.ViewModel.Goods;
using YapartMarket.WebApi.ViewModel.Goods.Cancel;

namespace YapartMarket.WebApi.Controllers
{
    /// <summary>
    /// Goods
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public sealed class GoodsController : ControllerBase
    {
        private IGoodsService _goodsService;

        /// <inheritdoc />
        public GoodsController(IGoodsService goodsService)
        {
            _goodsService = goodsService;
        }
        /// <summary>
        /// Add new order
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("order/new")]
        [Produces("application/json")]
        public async Task<IActionResult> New([FromBody] OrderNewViewModel order)
        {
            try
            {
                if (order != null)
                {
                    var shipmentId = order.OrderNewDataViewModel.Shipments[0].ShipmentId;
                    await _goodsService.SaveOrderAsync(order);
                    var result = await _goodsService.ProcessConfirmOrRejectAsync(shipmentId);
                    if (result.Succeeded)
                        return Ok();
                    else
                        return BadRequest(result.Errors);
                }
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }

            return Ok();
        }
        [HttpGet]
        [Route("CurrentDay")]
        [Produces("application/json")]
        public async Task<IActionResult> Get()
        {
            try
            {
                var ordersByDay = await _goodsService.GetOrderAsync(DateTime.Now.AddDays(-1).StartOfDay(), DateTime.Now.EndOfDay());
                if (ordersByDay != null)
                    return Ok(ordersByDay);
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        /// <summary>
        /// Cancel order
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("order/cancel")]
        [Produces("application/json")]
        public async Task<IActionResult> Cancel([FromBody] Cancel order)
        {
            if (order != null)
            {
                await _goodsService.CancelAsync(order);
                return Ok(new ConfirmCancel()
                {
                    data = new(),
                    success = 1,
                    meta = new()
                });
            }
            return BadRequest();
        }
    }
}
