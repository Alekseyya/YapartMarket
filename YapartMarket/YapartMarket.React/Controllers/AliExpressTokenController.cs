using System;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using YapartMarket.Core.BL;

namespace YapartMarket.React.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AliExpressTokenController : Controller
    {
        private readonly IAliExpressTokenService _aliExpressTokenService;

        public AliExpressTokenController(IAliExpressTokenService aliExpressTokenService)
        {
            _aliExpressTokenService = aliExpressTokenService;
        }

        [HttpGet]
        [Produces("application/json")]
        public IActionResult Get()
        {
            try
            {
                var aliExpressTokenInfoDto = _aliExpressTokenService.GetAccessToken();
                return Ok(JsonConvert.SerializeObject(aliExpressTokenInfoDto));
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }
    }
}
