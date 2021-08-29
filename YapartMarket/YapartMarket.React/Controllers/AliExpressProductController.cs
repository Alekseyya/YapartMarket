using System;
using System.Linq;
using System.Threading.Tasks;
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
        [Produces("application/json")]
        public IActionResult GetProductsInfo()
        {
            try
            {
                var products = _aliExpressProductService.GetProductsAliExpress();
                return Ok(products);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpGet]
        [Route("productsExcept")]
        [Produces("application/json")]
        public async Task<IActionResult> GetProductsExceptFromDatabase()
        {
            try
            {
                var products = _aliExpressProductService.GetProductsAliExpress();
                await _aliExpressProductService.ExceptProductsFromDataBase(products);
                return Ok(products);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("processDataFromAliExpress")]
        [Produces("application/json")]
        public async Task<IActionResult> ProcessDataFromAliExpress()
        {
            try
            {
                await _aliExpressProductService.ProcessDataFromAliExpress();
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpPatch]
        [Route("processUpdateProductsId")]
        [Produces("application/json")]
        public IActionResult ProcessUpdateProductsId()
        {
            try
            {
                var products = _aliExpressProductService.GetProductsAliExpress();
                return Ok(products);
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
