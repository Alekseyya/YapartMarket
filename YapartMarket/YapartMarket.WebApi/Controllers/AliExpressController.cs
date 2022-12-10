using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using YapartMarket.Core.BL;
using YapartMarket.Core.BL.AliExpress;
using YapartMarket.Core.DTO.AliExpress.OrderGetResponse;
using YapartMarket.Core.Extensions;
using YapartMarket.Core.Models.Azure;
using YapartMarket.WebApi.Model.AliExpress;

namespace YapartMarket.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AliExpressController : ControllerBase
    {
        private readonly IAliExpressOrderService _aliExpressOrderService;
        private readonly IMapper _mapper;

        public AliExpressController(IAliExpressOrderService aliExpressOrderService, IMapper mapper)
        {
            _aliExpressOrderService = aliExpressOrderService;
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
                    return Ok(_mapper.Map<IEnumerable<AliExpressOrder>, IEnumerable<Order>>(ordersByDay));
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
