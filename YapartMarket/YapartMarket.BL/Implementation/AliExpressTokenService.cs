using System.Collections.Generic;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Top.Api.Util;
using YapartMarket.Core.BL;
using YapartMarket.Core.Config;
using YapartMarket.Core.DTO;

namespace YapartMarket.BL.Implementation
{
    public class AliExpressTokenService : IAliExpressTokenService
    {
        private readonly AliExpressOptions _aliExpressOption;
        public AliExpressTokenService(IOptions<AliExpressOptions> options)
        {
            _aliExpressOption = options.Value;
        }

        public AliExpressTokenInfoDTO GetAccessToken()
        {
            WebUtils webUtils = new WebUtils();
            IDictionary<string, string> pout = new Dictionary<string, string>();
            pout.Add("grant_type", "authorization_code");
            pout.Add("client_id", _aliExpressOption.AppKey);
            pout.Add("client_secret", _aliExpressOption.AppSecret);
            pout.Add("sp", "ae");
            pout.Add("code", _aliExpressOption.AuthorizationCode);
            pout.Add("redirect_uri", _aliExpressOption.ReturnUrl);
            var output = webUtils.DoPost("https://oauth.aliexpress.com/token", pout);
            return JsonConvert.DeserializeObject<AliExpressTokenInfoDTO>(output);
        }
    }
}
