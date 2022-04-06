using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Top.Api;
using Top.Api.Request;
using YapartMarket.Core.BL;
using YapartMarket.Core.Config;
using YapartMarket.Core.Data.Interfaces.Azure;
using YapartMarket.Core.DTO;
using YapartMarket.Core.Extensions;
using YapartMarket.Core.Models.Azure;

namespace YapartMarket.BL.Implementation
{
    public sealed class AliExpressLogisticOrderDetailService : IAliExpressLogisticOrderDetailService
    {
        private readonly IOptions<AliExpressOptions> _options;
        private readonly IMapper _mapper;
        private readonly IAliExpressLogisticOrderDetailRepository aliExpressLogisticOrderDetailRepository;
        private readonly ITopClient _client;
        public AliExpressLogisticOrderDetailService(ILogger<AliExpressLogisticRedefiningService> logger,
            IOptions<AliExpressOptions> options,
            IMapper mapper, IAliExpressLogisticOrderDetailRepository aliExpressLogisticOrderDetailRepository)
        {
         
            _options = options;
            _mapper = mapper;
            this.aliExpressLogisticOrderDetailRepository = aliExpressLogisticOrderDetailRepository;
            _client = new DefaultTopClient(options.Value.HttpsEndPoint, options.Value.AppKey, options.Value.AppSecret, "Json");
        }

        public List<AliExpressLogisticsOrderDetailDto> GetLogisticOrderDetailRequest(long orderId)
        {
            var req = new AliexpressLogisticsQuerylogisticsorderdetailRequest();
            req.TradeOrderId = orderId;
            req.PageSize = 20;
            req.CurrentPage = 1;
            var rsp = _client.Execute(req, _options.Value.AccessToken);
            var aliExpressOrderDetailDTO = JsonConvert.DeserializeObject<AliExpressLogisticsOrderDetailResponseRoot>(rsp.Body);
            return aliExpressOrderDetailDTO.AliExpressLogisticsOrderDetailResponse.AliExpressLogisticsOrderDetailResponseResult
                .AliExpressLogisticsOrderDetailResultList.AliExpressLogisticsOrderDetailDtos;
        }

        public async Task ProcessLogisticsOrderDetailAsync(List<AliExpressLogisticsOrderDetailDto> aliExpressLogisticsOrderDetailDtos)
        {
            var logisticOrderDetails = _mapper.Map<List<AliExpressLogisticsOrderDetailDto>, List<AliExpressLogisticOrderDetail>>(aliExpressLogisticsOrderDetailDtos);
            var logisticOrderDetailsDb = await aliExpressLogisticOrderDetailRepository.GetInAsync("order_id", new { order_id = logisticOrderDetails.Select(x => x.OrderId) });
            var newLogisticOrderDetails = logisticOrderDetails.Where(orderLogistic => logisticOrderDetailsDb.All(orderDb => orderDb.OrderId != orderLogistic.OrderId));
            if (newLogisticOrderDetails.Any())
            {
                var insertOrder = new AliExpressLogisticOrderDetail().InsertString("dbo.logistic_order_detail");
                await aliExpressLogisticOrderDetailRepository.InsertAsync(insertOrder, newLogisticOrderDetails.Select(x => new
                {
                    order_id = x.OrderId,
                    logistic_order_id = x.LogisticOrderId,
                    out_order_code = x.OutOrderCode
                }));
            }
        }
        public async Task<AliExpressLogisticOrderDetail> GetDetail(long orderId)
        {
            var orderDetail = await aliExpressLogisticOrderDetailRepository.GetAsync("select * from dbo.logistic_order_detail where order_id = @order_id", new { order_id = orderId });
            return orderDetail.FirstOrDefault();
        }
    }
}
