using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Top.Api;
using Top.Api.Request;
using Top.Api.Response;
using YapartMarket.Core.Config;
using YapartMarket.Core.DTO;
using YapartMarket.Core.Models.Azure;

namespace YapartMarket.BL.Implementation
{
    public sealed class AliExpressLogisticOrderDetailService
    {
        private readonly IOptions<AliExpressOptions> _options;
        private readonly IMapper _mapper;
        private readonly ITopClient _client;
        public AliExpressLogisticOrderDetailService(ILogger<AliExpressLogisticRedefiningService> logger,
            IOptions<AliExpressOptions> options,
            IMapper mapper)
        {
         
            _options = options;
            _mapper = mapper;
            _client = new DefaultTopClient(options.Value.HttpsEndPoint, options.Value.AppKey, options.Value.AppSecret, "Json");
        }

        public List<AliExpressLogisticsOrderDetailDto> GetLogisticOrderDetail(long orderId)
        {
            var req = new AliexpressLogisticsQuerylogisticsorderdetailRequest();
            req.TradeOrderId = orderId;
            AliexpressLogisticsQuerylogisticsorderdetailResponse rsp = _client.Execute(req, _options.Value.AccessToken);
            var aliExpressOrderDetailDTO = JsonConvert.DeserializeObject<AliExpressLogisticsOrderDetailResponseRoot>(rsp.Body);
            return aliExpressOrderDetailDTO.AliExpressLogisticsOrderDetailResponse.AliExpressLogisticsOrderDetailResponseResult
                .AliExpressLogisticsOrderDetailResultList.AliExpressLogisticsOrderDetailDtos;
        }

        public Task ProcessLogisticsOrderDetail(List<AliExpressLogisticsOrderDetailDto> aliExpressLogisticsOrderDetailDtos)
        {
            var aliExpressLogisticOrderDetail =
                _mapper.Map<List<AliExpressLogisticsOrderDetailDto>, List<AliExpressLogisticOrderDetail>>(
                    aliExpressLogisticsOrderDetailDtos);
            return Task.CompletedTask;
        }
    }
}
