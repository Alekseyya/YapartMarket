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
        private readonly IWritableOptions<AliExpressOptions> _writableOptions;
        
        public AliExpressTokenController(IWritableOptions<AliExpressOptions> writableOptions)
        {
            _writableOptions = writableOptions;
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
