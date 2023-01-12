using Microsoft.AspNetCore.Mvc;
using YapartMarket.Core.DateStructures;
using YapartMarket.Core.Extensions;
using YapartMarket.WebApi.Services.Interfaces;
using YapartMarket.WebApi.ViewModel.Goods;

namespace YapartMarket.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public sealed class GoodsController : Controller
    {
        private IConfiguration _configuration;
        private IGoodsService _goodsService;

        public GoodsController(IConfiguration configuration, IGoodsService goodsService)
        {
            _configuration = configuration;
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
                var orders = await _goodsService.GetOrderAsync(order);
                await _goodsService.SaveOrderAsync(order);
                //if (orderId != default)
                //{
                //    if (orders.IsAny())
                //    {
                //        if (orders.All(x => x.ReasonType == ReasonType.Empty))
                //            await _goodsService.Confirm(shipmentId, orderId);
                //        if (orders.All(x => x.ReasonType == ReasonType.OUT_OF_STOCK))
                //            await _goodsService.Reject(shipmentId, orderId);
                //        if (orders.Any(x => x.ReasonType == ReasonType.Empty) && orders.Any(x => x.ReasonType == ReasonType.OUT_OF_STOCK))
                //        {
                //            await _goodsService.Confirm(shipmentId, orderId);
                //            await _goodsService.Reject(shipmentId, orderId);
                //        }
                //        var isPackage = await _goodsService.Package(shipmentId, orderId); //дописать ошибки, если она появятся
                //        if (isPackage)
                //        {
                //            await _goodsService.Shipment(shipmentId);
                //        }
                //    }
                //}
                return Ok(new SuccessfulResponse()
                {
                    Success = 1
                });
            }
            return BadRequest();
        }
    }
}
