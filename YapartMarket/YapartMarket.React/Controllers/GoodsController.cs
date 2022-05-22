using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YapartMarket.React.Services.Interfaces;
using YapartMarket.React.ViewModels.Goods;

namespace YapartMarket.React.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GoodsController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly IGoodsService _goodsService;

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
                _goodsService.GetOrders(order, out List<OrderNewShipmentItem> confirmOrders, out List<OrderNewShipmentItem> rejectOrders);
                var orderId = await _goodsService.SaveOrder(shipmentId, confirmOrders, rejectOrders);
                if (orderId != default)
                {
                    if (confirmOrders.Any() && !rejectOrders.Any())
                        await _goodsService.Confirm(shipmentId, orderId);
                    if (!confirmOrders.Any() && rejectOrders.Any())
                        await _goodsService.Reject(shipmentId, orderId);
                    if (confirmOrders.Any() && rejectOrders.Any())
                    {
                        await _goodsService.Confirm(shipmentId, orderId);
                        await _goodsService.Reject(shipmentId, orderId);
                    }
                    await _goodsService.Package(shipmentId, orderId);
                }
                return Ok(new SuccessfulResponse()
                {
                    Success = 1
                });
            }
            return BadRequest();
        }
    }
}
