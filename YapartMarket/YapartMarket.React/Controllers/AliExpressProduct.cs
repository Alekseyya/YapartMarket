using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Top.Api;
using Top.Api.Request;
using Top.Api.Response;
using YapartMarket.Core.BL;
using YapartMarket.Core.Config;

namespace YapartMarket.React.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AliExpressProduct : Controller
    {
        private readonly IAliExpressTokenService _aliExpressTokenService;
        private readonly AliExpressOptions _option;

        public AliExpressProduct(IOptions<AliExpressOptions> option, IAliExpressTokenService aliExpressTokenService)
        {
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
                ITopClient client = new DefaultTopClient("https://eco.taobao.com/router/rest", _option.AppKey, _option.AppSecret);
                AliexpressSolutionProductListGetRequest req = new AliexpressSolutionProductListGetRequest();
                AliexpressSolutionProductListGetRequest.ItemListQueryDomain obj1 = new AliexpressSolutionProductListGetRequest.ItemListQueryDomain();
                var aliExpressTokenInfoDto = _aliExpressTokenService.GetAccessToken();
                AliexpressSolutionProductListGetResponse rsp = null;
                if (aliExpressTokenInfoDto != null && !string.IsNullOrEmpty(aliExpressTokenInfoDto.AccessToken))
                    rsp = client.Execute(req, aliExpressTokenInfoDto.AccessToken);
                return Ok(rsp.Body);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
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
                ITopClient client = new DefaultTopClient("https://eco.taobao.com/router/rest", _option.AppKey, _option.AppSecret);
                var req = new AliexpressSolutionProductInfoGetRequest();
                req.ProductId = productId;
                var aliExpressTokenInfoDto = _aliExpressTokenService.GetAccessToken();
                AliexpressSolutionProductInfoGetResponse rsp = null;
                if (aliExpressTokenInfoDto != null && !string.IsNullOrEmpty(aliExpressTokenInfoDto.AccessToken))
                    rsp = client.Execute(req, aliExpressTokenInfoDto.AccessToken);
                return Ok(rsp.Body);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

        }

    }
}
