using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using YapartMarket.Core.Config;
using YapartMarket.React.Options;

namespace YapartMarket.React.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AliExpressAuthorizeCodeController : Controller
    {
        private readonly IWritableOptions<AliExpressOptions> _writableOptions;
        private readonly IConfiguration _configuration;

        public AliExpressAuthorizeCodeController(IWritableOptions<AliExpressOptions> writableOptions, IConfiguration configuration)
        {
            _writableOptions = writableOptions;
            _configuration = configuration;
        }

        [HttpGet]
        [Route("getCode")]
        [Produces("application/json")]
        public IActionResult GetCode(string passToken)
        {
            if (!string.IsNullOrEmpty(passToken) && _configuration.GetSection("PassAliExpressCode").Value == passToken)
            {
                return Ok(_writableOptions.Value.AuthorizationCode);
            }

            return StatusCode(500);
        }

        [HttpPost]
        //[Produces("application/json")]
        [Route("SetCode")]
        public IActionResult SetCode(string code)
        {
            if (!string.IsNullOrEmpty(code))
            {
                try
                {
                    _writableOptions.Update(opt =>
                    {
                        opt.AuthorizationCode = code;
                    });
                }
                catch (Exception e)
                {
                    return BadRequest(e.Message);
                }
                return StatusCode(200);
            }
            return BadRequest("Не указан код");
        }
    }
}
