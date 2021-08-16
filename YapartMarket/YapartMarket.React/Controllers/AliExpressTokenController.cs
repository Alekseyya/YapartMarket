using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using YapartMarket.Core.Config;
using YapartMarket.React.Options;

namespace YapartMarket.React.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AliExpressTokenController : Controller
    {
        private readonly IOptions<AliExpressOptions> _options;
        private readonly IWritableOptions<AliExpressOptions> _writableOptions;
        private readonly AliExpressOptions _aliExpressOptions;
        
        public AliExpressTokenController(IOptions<AliExpressOptions> options, IWritableOptions<AliExpressOptions> writableOptions)
        {
            _options = options;
            _writableOptions = writableOptions;
            _aliExpressOptions = options.Value;
        }

        [HttpPost]
        [Produces("application/json")]
        public IActionResult Token(string code)
        {
            if (!string.IsNullOrEmpty(code))
            {
                _writableOptions.Update(opt =>
                {
                    opt.AuthorizationCode = code;
                });
                return Ok();
            }
            return BadRequest();
        }
    }
}
