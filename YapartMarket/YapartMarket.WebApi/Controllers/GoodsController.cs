using Microsoft.AspNetCore.Mvc;
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
                    await _goodsService.ProcessConfirmOrRejectAsync(shipmentId);
                    return Ok(new SuccessfulResponse()
                    {
                        Success = 1
                    });
                }
                return BadRequest("1111");
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
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
                return Ok(new SuccessfulResponse()
                {
                    Success = 1
                });
            }
            return BadRequest();
        }
    }
}
