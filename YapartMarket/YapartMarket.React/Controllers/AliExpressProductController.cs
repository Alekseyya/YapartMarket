using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using YapartMarket.BL.Implementation;
using YapartMarket.Core.BL;
using YapartMarket.Core.Config;
using YapartMarket.Core.Models.Azure;

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

        [HttpPost]
        [Route("processDataFromAliExpress")]
        [Produces("application/json")]
        public async Task<IActionResult> ProcessDataFromAliExpress()
        {
            try
            {
                await _aliExpressProductService.ProcessDataFromAliExpress();
                await _aliExpressProductService.ProcessUpdateDatabaseAliExpressProductId();
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
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
                var productInfo =_aliExpressProductService.GetProductInfo(productId);
                return Ok(productInfo);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPut]
        [Route("updateProductInventory")]
        [Produces("application/json")]
        public IActionResult UpdateProductInventory(long productId, string sku, int inventory)
        {
            try
            {
                if (productId == 0)
                    return BadRequest("Не указан productId");
                if(string.IsNullOrEmpty(sku))
                    return BadRequest("Не указан sku");

                var products = new List<Product>
                {
                    new()
                    {
                        Sku = sku,
                        Count = inventory,
                        AliExpressProductId = productId
                    }
                };
                _aliExpressProductService.UpdateInventoryProducts(products);
                var productInfo = _aliExpressProductService.GetProductInfo(productId);
                return Ok(productInfo);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPatch]
        [Route("updateProductsInventory")]
        [Produces("application/json")]
        public async Task<IActionResult> UpdateProductsInventory()
        {
            try
            {
               var products = await _aliExpressProductService.ListProductsForUpdateInventory();
               _aliExpressProductService.UpdateInventoryProducts(products);
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
