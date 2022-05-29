using AutoMapper;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Top.Api;
using Top.Api.Request;
using YapartMarket.Core.BL.AliExpress;
using YapartMarket.Core.Config;
using YapartMarket.Core.DTO.AliExpress.FullOrderInfo;

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
        public Root GetRequest(long orderId, long? flag = null)
        {
            var req = new AliexpressTradeNewRedefiningFindorderbyidRequest();
            AliexpressTradeNewRedefiningFindorderbyidRequest.AeopTpSingleOrderQueryDomain obj1 = new AliexpressTradeNewRedefiningFindorderbyidRequest.AeopTpSingleOrderQueryDomain();
            obj1.OrderId = orderId;
            req.Param1_ = obj1;
            var rsp = _client.Execute(req, _options.Value.AccessToken);
            return JsonConvert.DeserializeObject<Root>(rsp.Body);
        }
    }
}
