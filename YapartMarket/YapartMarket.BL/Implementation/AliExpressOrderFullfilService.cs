using Microsoft.Extensions.Options;
using Top.Api;
using Top.Api.Request;
using YapartMarket.Core.BL;
using YapartMarket.Core.Config;

namespace YapartMarket.BL.Implementation
{
    public sealed class AliExpressOrderFullfilService : IAliExpressOrderFullfilService
    {
        private readonly IOptions<AliExpressOptions> _options;
        private readonly ITopClient _client;

        public AliExpressOrderFullfilService(IOptions<AliExpressOptions> options)
        {
            _options = options;
            _client = new DefaultTopClient(options.Value.HttpsEndPoint, options.Value.AppKey, options.Value.AppSecret, "Json");
        }

        public bool OrderFullfil(string service, long orderId, long logisticNumber)
        {
            var req = new AliexpressSolutionOrderFulfillRequest();
            req.ServiceName = service;
            req.OutRef = orderId.ToString();
            req.SendType = "all";
            req.LogisticsNo = logisticNumber.ToString();
            var rsp = _client.Execute(req, _options.Value.AccessToken);
            return rsp.IsError;
        }
    }
}
