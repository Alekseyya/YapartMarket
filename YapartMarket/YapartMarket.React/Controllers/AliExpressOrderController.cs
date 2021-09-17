﻿using System;
using System.Collections.Generic;
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
        private readonly IMapper _mapper;

        public AliExpressOrderController(IAliExpressOrderService aliExpressOrderService, IMapper mapper)
        {
            _aliExpressOrderService = aliExpressOrderService;
            _mapper = mapper;
        }

        [HttpGet]
        [Produces("application/json")]
        public async Task<IActionResult> Get()
        {
            var dateTimeNow = DateTime.Now.AddDays(-1);
            var ordersByDay = await _aliExpressOrderService.GetOrders(dateTimeNow.StartOfDay(), dateTimeNow.EndOfDay());
            if (ordersByDay.IsAny())
                return Ok(_mapper.Map<IEnumerable<AliExpressOrder>, IEnumerable<AliExpressOrderViewModel>>(ordersByDay));
            return Ok();
        }

        [HttpPost]
        [Route("downloadNewOrders")]
        public async Task<IActionResult> DownloadNewOrders()
        {
            try
            {
                var aliExpressOrderDTO = _aliExpressOrderService.QueryOrderDetail(new DateTime(2021, 09, 01).StartOfDay(), DateTime.Today.EndOfDay());
                var aliExpressOrders = _mapper.Map<List<AliExpressOrderDTO>, List<AliExpressOrder>>(aliExpressOrderDTO);
                await _aliExpressOrderService.AddOrders(aliExpressOrders);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
            return StatusCode(200);
        }
    }
}
