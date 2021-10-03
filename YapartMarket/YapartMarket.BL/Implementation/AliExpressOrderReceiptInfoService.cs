using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Top.Api;
using Top.Api.Request;
using Top.Api.Response;
using YapartMarket.Core.BL;
using YapartMarket.Core.Config;
using YapartMarket.Core.Data.Interfaces.Azure;
using YapartMarket.Core.DTO;
using YapartMarket.Core.Models.Azure;

namespace YapartMarket.BL.Implementation
{
    public class AliExpressOrderReceiptInfoService : IAliExpressOrderReceiptInfoService
    {
        private readonly Logger<AliExpressOrderReceiptInfoService> _logger;
        private readonly IAzureAliExpressOrderReceiptInfoRepository _orderReceiptInfoRepository;
        private readonly IMapper _mapper;
        private readonly AliExpressOptions _options;
        private readonly ITopClient _client;
        public AliExpressOrderReceiptInfoService(Logger<AliExpressOrderReceiptInfoService> logger, IOptions<AliExpressOptions> options,
            IAzureAliExpressOrderReceiptInfoRepository orderReceiptInfoRepository, IMapper mapper)
        {
             _client = new DefaultTopClient(options.Value.HttpsEndPoint, options.Value.AppKey, options.Value.AppSecret, "Json");
            _logger = logger;
            _orderReceiptInfoRepository = orderReceiptInfoRepository;
            _mapper = mapper;
            _options = options.Value;
        }

        public AliExpressOrderReceiptInfoDTO GetReceiptInfo(long orderId)
        {
            AliexpressSolutionOrderReceiptinfoGetRequest req = new AliexpressSolutionOrderReceiptinfoGetRequest();
            AliexpressSolutionOrderReceiptinfoGetRequest.SingleOrderQueryDomain singleOrderQueryDomain = new AliexpressSolutionOrderReceiptinfoGetRequest.SingleOrderQueryDomain();
            singleOrderQueryDomain.OrderId = orderId;
            req.Param1_ = singleOrderQueryDomain;
            AliexpressSolutionOrderReceiptinfoGetResponse rsp = _client.Execute(req, _options.AccessToken);
            var aliExpressReceiptRoot = JsonConvert.DeserializeObject<AliExpressReceiptRoot>(rsp.Body);
            return aliExpressReceiptRoot?.AliExpressReceiptInfoResult.AliExpressOrderReceiptInfoDto;
        }

        public async Task InsertOrderReceipt(AliExpressOrderReceiptInfoDTO orderInfoDto)
        {
            var aliExpressOrderReceiptInfo = _mapper.Map<AliExpressOrderReceiptInfoDTO, AliExpressOrderReceiptInfo>(orderInfoDto);
            await _orderReceiptInfoRepository.InsertAsync(aliExpressOrderReceiptInfo);
        }
    }
}
