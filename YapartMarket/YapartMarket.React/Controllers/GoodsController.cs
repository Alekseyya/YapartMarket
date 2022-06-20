using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YapartMarket.Core.DateStructures;
using YapartMarket.Core.Extensions;
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
                var orders = await _goodsService.GetOrders(order);
                var orderId = await _goodsService.SaveOrder(shipmentId, orders);
                if (orderId != default)
                {
                    if (orders.IsAny())
                    {
                        if (orders.All(x => x.ReasonType == ReasonType.Empty))
                            await _goodsService.Confirm(shipmentId, orderId);
                        if (orders.All(x => x.ReasonType == ReasonType.OUT_OF_STOCK))
                            await _goodsService.Reject(shipmentId, orderId);
                        if (orders.Any(x => x.ReasonType == ReasonType.Empty) && orders.Any(x => x.ReasonType == ReasonType.OUT_OF_STOCK))
                        {
                            await _goodsService.Confirm(shipmentId, orderId);
                            await _goodsService.Reject(shipmentId, orderId);
                        }
                        var isPackage = await _goodsService.Package(shipmentId, orderId);
                        if (isPackage)
                        {

                        }
                    }
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
