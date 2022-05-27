using AutoMapper;
using Microsoft.Extensions.Options;
using Top.Api;
using Top.Api.Request;
using Top.Api.Response;
using YapartMarket.Core.BL.AliExpress;
using YapartMarket.Core.Config;

namespace YapartMarket.BL.Implementation.AliExpress
{
    public sealed class CategoryTreeService : ICategoryTreeService
    {
        private readonly IOptions<AliExpressOptions> _options;
        private readonly IMapper _mapper;
        private readonly DefaultTopClient _client;

        public CategoryTreeService(IOptions<AliExpressOptions> options, IMapper mapper)
        {
            _options = options;
            _mapper = mapper;
            _client = new DefaultTopClient(options.Value.HttpsEndPoint, options.Value.AppKey, options.Value.AppSecret, "Json");
        }
        public string GetRequest(long categoryId)
        {
            var req = new AliexpressSolutionSellerCategoryTreeQueryRequest();
            req.CategoryId = categoryId;
            req.FilterNoPermission = true;
            AliexpressSolutionSellerCategoryTreeQueryResponse rsp = _client.Execute(req, _options.Value.AccessToken);
            return rsp.Body;
        }
    }
}
