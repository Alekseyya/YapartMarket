using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using YapartMarket.React.Services.Interfaces;
using YapartMarket.React.ViewModels.Goods;
using Product = YapartMarket.Core.Models.Azure.Product;

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
        public IActionResult NewOrder([FromBody] OrderNewViewModel order)
        {
            if (order != null)
            {
                var orderItems = order.OrderNewDataViewModel.Shipments[0].Items;
                _goodsService.GetOrders(order, out List<OrderNewShipmentItem> confirmOrders, out List<OrderNewShipmentItem> rejectOrders);
                if (confirmOrders.Any() && !rejectOrders.Any())
                    _goodsService.Confirm(order.OrderNewDataViewModel.Shipments[0].ShipmentId, confirmOrders);
                return Ok(new SuccessfulResponse()
                {
                    Success = 1
                });
            }
            return BadRequest();
        }
    }
}
