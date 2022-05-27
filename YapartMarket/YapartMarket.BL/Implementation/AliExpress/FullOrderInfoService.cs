using AutoMapper;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Top.Api;
using Top.Api.Request;
using Top.Api.Response;
using YapartMarket.Core.BL.AliExpress;
using YapartMarket.Core.Config;

namespace YapartMarket.BL.Implementation.AliExpress
{
    public sealed class FullOrderInfoService : IFullOrderInfoService
    {
        private readonly IOptions<AliExpressOptions> _options;
        private readonly IMapper _mapper;
        private readonly ITopClient _client;

        public FullOrderInfoService(ILogger<FullOrderInfoService> logger,
            IOptions<AliExpressOptions> options, IMapper mapper)
        {
            _options = options;
            _mapper = mapper;
            _client = new DefaultTopClient(options.Value.HttpsEndPoint, options.Value.AppKey, options.Value.AppSecret, "Json");
        }
        public string GetRequest(long orderId, long? flag = null)
        {
            var req = new AliexpressTradeNewRedefiningFindorderbyidRequest();
            AliexpressTradeNewRedefiningFindorderbyidRequest.AeopTpSingleOrderQueryDomain obj1 = new AliexpressTradeNewRedefiningFindorderbyidRequest.AeopTpSingleOrderQueryDomain();
            obj1.OrderId = orderId;
            req.Param1_ = obj1;
            AliexpressTradeNewRedefiningFindorderbyidResponse rsp = _client.Execute(req, _options.Value.AccessToken);
            return rsp.Body;
        }
    }
}
