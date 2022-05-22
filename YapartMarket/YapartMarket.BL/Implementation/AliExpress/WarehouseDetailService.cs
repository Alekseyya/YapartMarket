using System;
using AutoMapper;
using Microsoft.Extensions.Options;
using Top.Api;
using Top.Api.Request;
using YapartMarket.Core.BL.AliExpress;
using YapartMarket.Core.Config;

namespace YapartMarket.BL.Implementation.AliExpress
{
    public class WarehouseDetailService : IWarehouseDetailService
    {
        private readonly IOptions<AliExpressOptions> _options;
        private readonly IMapper _mapper;
        private readonly ITopClient _client;

        public WarehouseDetailService(IOptions<AliExpressOptions> options, IMapper mapper)
        {
            _options = options;
            _mapper = mapper;
            _client = new DefaultTopClient(options.Value.HttpsEndPoint, options.Value.AppKey, options.Value.AppSecret, "Json");
        }
        public void /*List<DomesticLogicalCompanyInfo>*/ GetRequest(long orderId)
        {
            var req = new AliexpressLogisticsWarehouseQuerydetailRequest();
            //req.ConsignType = "DOMESTIC";
            req.TradeOrderId = orderId;
            var rsp = _client.Execute(req, _options.Value.AccessToken);
            Console.WriteLine(rsp.Body);
            //var result = JsonConvert.DeserializeObject<RedefiningDomesticLogicalCompanyRoot>(rsp.Body);
            //return result.Result.DomesticLogicalCompanyList.DomesticLogicalCompanyInfo;
        }
    }
}
