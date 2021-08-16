using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using YapartMarket.Core.Config;

namespace YapartMarket.React.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AliExpressTokenController : Controller
    {
        private readonly AliExpressOptions _aliExpressOptions;
        public AliExpressTokenController(IOptions<AliExpressOptions> options)
        {
            _aliExpressOptions = options.Value;
        }

        [HttpPost]
        [Produces("application/json")]
        public IActionResult Token(string code)
        {
            if (!string.IsNullOrEmpty(code))
            {
                _aliExpressOptions.AuthorizationCode = code;
                return Ok();
            }
            return BadRequest();
        }
    }
}
