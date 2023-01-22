using Microsoft.AspNetCore.Mvc;
using YapartMarket.WebApi.Services.Interfaces;
using YapartMarket.WebApi.ViewModel.Goods;
using YapartMarket.WebApi.ViewModel.Goods.Cancel;

namespace YapartMarket.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public sealed class GoodsController : Controller
    {
        private IGoodsService _goodsService;

        public GoodsController(IGoodsService goodsService)
        {
            _goodsService = goodsService;
        }
        [HttpPost]
        [Route("order/new")]
        [Produces("application/json")]
        public async Task<IActionResult> NewOrder([FromBody] OrderNewViewModel order)
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
            return BadRequest();
        }
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
