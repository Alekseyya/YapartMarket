using System.Linq;
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
        private readonly IAzureAliExpressOrderReceiptInfoRepository _orderReceiptInfoRepository;
        private readonly IMapper _mapper;
        private readonly AliExpressOptions _options;
        private readonly ITopClient _client;
        public AliExpressOrderReceiptInfoService(IOptions<AliExpressOptions> options,
            IAzureAliExpressOrderReceiptInfoRepository orderReceiptInfoRepository, IMapper mapper)
        {
             _client = new DefaultTopClient(options.Value.HttpsEndPoint, options.Value.AppKey, options.Value.AppSecret, "Json");
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

        public async Task InsertOrderReceipt(long orderId, AliExpressOrderReceiptInfoDTO orderInfoDto)
        {
            var orderReceiptInDb = await _orderReceiptInfoRepository.GetAsync("select * from dbo.order_receipt_infos where order_id = @order_id", new { order_id = orderId });
            if (!orderReceiptInDb.Any())
            {
                var aliExpressOrderReceiptInfo = _mapper.Map<AliExpressOrderReceiptInfoDTO, AliExpressOrderReceiptInfo>(orderInfoDto);
                aliExpressOrderReceiptInfo.OrderId = orderId;
                await _orderReceiptInfoRepository.InsertAsync(aliExpressOrderReceiptInfo);
            }
        }
    }
}
