using AutoMapper;
using Microsoft.Extensions.Options;
using Top.Api;
using Top.Api.Request;
using Top.Api.Response;
using YapartMarket.Core.BL.AliExpress;
using YapartMarket.Core.Config;

namespace YapartMarket.BL.Implementation.AliExpress
{
    public sealed class AttributeService : IAttributeService
    {
        private readonly IOptions<AliExpressOptions> _options;
        private readonly IMapper _mapper;
        private readonly DefaultTopClient _client;

        public AttributeService(IOptions<AliExpressOptions> options, IMapper mapper)
        {
            _options = options;
            _mapper = mapper;
            _client = new DefaultTopClient(options.Value.HttpsEndPoint, options.Value.AppKey, options.Value.AppSecret, "Json");
        }
        public string GetRequest(long categoryId, string? locale = null)
        {
            var req = new AliexpressCategoryRedefiningGetallchildattributesresultRequest();
            req.CateId = categoryId;
            if (locale != null)
                req.Locale = locale;
            AliexpressCategoryRedefiningGetallchildattributesresultResponse rsp = _client.Execute(req, _options.Value.AccessToken);
            return rsp.Body;
        }
    }
}
