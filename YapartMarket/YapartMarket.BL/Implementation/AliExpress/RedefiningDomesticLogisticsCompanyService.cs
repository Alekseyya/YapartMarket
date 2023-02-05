using System.Collections.Generic;
using AutoMapper;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Top.Api;
using Top.Api.Request;
using YapartMarket.Core.BL.AliExpress;
using YapartMarket.Core.Config;
using YapartMarket.Core.DTO.AliExpress.RedefininDomesticLogicalCompany;

namespace YapartMarket.BL.Implementation.AliExpress
{
    public class RedefiningDomesticLogisticsCompanyService : IRedefiningDomesticLogisticsCompany
    {
        private readonly IOptions<AliExpressOptions> _options;
        private readonly IMapper _mapper;
        private readonly ITopClient _client;

        public RedefiningDomesticLogisticsCompanyService(ILogger<RedefiningDomesticLogisticsCompanyService> logger,
            IOptions<AliExpressOptions> options, IMapper mapper)
        {
            _options = options;
            _mapper = mapper;
            _client = new DefaultTopClient(options.Value.HttpsEndPoint, options.Value.AppKey, options.Value.AppSecret, "Json");
        }
        public List<DomesticLogicalCompanyInfo> GetRequest()
        {

            var req = new AliexpressLogisticsRedefiningQureywlbdomesticlogisticscompanyRequest();
            var rsp = _client.Execute(req, _options.Value.AccessToken);
            var result = JsonConvert.DeserializeObject<RedefiningDomesticLogicalCompanyRoot>(rsp.Body);
            return result.Result.DomesticLogicalCompanyList.DomesticLogicalCompanyInfo;
        }
    }
}
