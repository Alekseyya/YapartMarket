﻿using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using YapartMarket.Core.BL;
using YapartMarket.Core.Config;

namespace YapartMarket.React.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AliExpressProductController : Controller
    {
        private readonly IAliExpressProductService _aliExpressProductService;
        private readonly IAliExpressTokenService _aliExpressTokenService;
        private readonly AliExpressOptions _option;

        public AliExpressProductController(IOptions<AliExpressOptions> option, IAliExpressProductService aliExpressProductService, IAliExpressTokenService aliExpressTokenService)
        {
            _aliExpressProductService = aliExpressProductService;
            _aliExpressTokenService = aliExpressTokenService;
            _option = option.Value;
        }

        [HttpGet]
        [Route("productsInfo")]
        //[Produces("application/json")]
        public IActionResult GetProductsInfo()
        {
            try
            {
                var productsJson = _aliExpressProductService.GetProducts();
                return Ok(productsJson);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet]
        [Route("productInfo")]
        [Produces("application/json")]
        public IActionResult GetProductInfo(long productId)
        {
            try
            {
                if (productId == 0)
                    return BadRequest("Не указан productId");
                var productInfo =_aliExpressProductService.GetProduct(productId);
                return Ok(productInfo);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

    }
}
