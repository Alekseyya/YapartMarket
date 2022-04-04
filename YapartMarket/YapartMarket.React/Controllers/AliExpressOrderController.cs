using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using YapartMarket.Core.BL;
using YapartMarket.Core.DTO;
using YapartMarket.Core.Extensions;
using YapartMarket.Core.Models.Azure;
using YapartMarket.React.ViewModels;

namespace YapartMarket.React.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AliExpressOrderController : Controller
    {
        private readonly IAliExpressOrderService _aliExpressOrderService;
        private readonly IAliExpressOrderReceiptInfoService _aliExpressOrderReceiptInfoService;
        private readonly IMapper _mapper;

        public AliExpressOrderController(IAliExpressOrderService aliExpressOrderService, IAliExpressOrderReceiptInfoService aliExpressOrderReceiptInfoService,  IMapper mapper)
        {
            _aliExpressOrderService = aliExpressOrderService;
            _aliExpressOrderReceiptInfoService = aliExpressOrderReceiptInfoService;
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
                    return Ok(_mapper.Map<IEnumerable<AliExpressOrder>, IEnumerable<AliExpressOrderViewModel>>(ordersByDay));
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
                return Ok(_mapper.Map<IEnumerable<AliExpressOrder>, IEnumerable<AliExpressOrderViewModel>>(ordersByDay));
            return Ok();
        }

        [HttpPut]
        [Route("UpdateOrders")]
        [Produces("application/json")]
        public async Task<IActionResult> UpdateOrders()
        {
            try
            {
                var dateTimeNow = DateTime.UtcNow;
                var ordersDTO = _aliExpressOrderService.QueryOrderDetail(dateTimeNow.AddDays(-1).StartOfDay(), dateTimeNow.AddDays(+1).EndOfDay());
                if (ordersDTO.Any())
                {
                    var aliExpressOrders = _mapper.Map<List<AliExpressOrderDTO>, List<AliExpressOrder>>(ordersDTO);
                    await _aliExpressOrderService.AddOrders(aliExpressOrders);
                    if (aliExpressOrders.Any())
                    {
                        foreach (var aliExpressOrder in aliExpressOrders)
                        {
                            var orderReceiptDto = _aliExpressOrderReceiptInfoService.GetReceiptInfo(aliExpressOrder.OrderId);
                            await _aliExpressOrderReceiptInfoService.InsertOrderReceipt(aliExpressOrder.OrderId, orderReceiptDto);
                        }
                    }
                }
            }
            catch (Exception e)
            {

                return BadRequest(e.Message + "\n" + e.StackTrace);
            }
            return Ok();
        }

        [HttpPost]
        [Route("downloadNewOrders")]
        public async Task<IActionResult> DownloadNewOrders()
        {
            try
            {
                //var yesterdayDateTime = new DateTimeWithZone(DateTime.Now.AddDays(-1).StartOfDay(), TimeZoneInfo.FindSystemTimeZoneById("Russian Standard Time"));
                //var tomorrowDateTime = new DateTimeWithZone(DateTime.Now.AddDays(+1).EndOfDay(), TimeZoneInfo.FindSystemTimeZoneById("Russian Standard Time"));
                var aliExpressOrderDTO = _aliExpressOrderService.QueryOrderDetail(DateTime.Now.AddDays(-1).StartOfDay(), DateTime.Now.AddDays(+1).EndOfDay());
                var aliExpressOrders = _mapper.Map<List<AliExpressOrderDTO>, List<AliExpressOrder>>(aliExpressOrderDTO);
                await _aliExpressOrderService.AddOrders(aliExpressOrders);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message + "\n" + e.StackTrace);
            }
            return StatusCode(200);
        }
    }
}
