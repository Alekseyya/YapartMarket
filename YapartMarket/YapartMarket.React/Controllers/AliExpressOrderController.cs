using System;
using Microsoft.AspNetCore.Mvc;
using YapartMarket.Core.BL;
using YapartMarket.Core.Extensions;

namespace YapartMarket.React.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AliExpressOrderController : Controller
    {
        private readonly IAliExpressOrderService _aliExpressOrderService;

        public AliExpressOrderController(IAliExpressOrderService aliExpressOrderService)
        {
            _aliExpressOrderService = aliExpressOrderService;
        }
        [HttpPost]
        [Route("downloadNewOrders")]
        public IActionResult DownloadNewOrders()
        {
            try
            {
                var orders = _aliExpressOrderService.QueryOrderDetail(new DateTime(2021, 09, 01).StartOfDay(), DateTime.Today.EndOfDay());
                _aliExpressOrderService.AddOrders(orders);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
            return StatusCode(200);
        }
    }
}
