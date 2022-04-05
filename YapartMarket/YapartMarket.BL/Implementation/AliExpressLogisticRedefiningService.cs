using System.Collections.Generic;
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
using YapartMarket.Core.Extensions;
using YapartMarket.Core.Models.Azure;

namespace YapartMarket.BL.Implementation
{
    public class AliExpressLogisticRedefiningService : IAliExpressLogisticRedefiningService
    {
        private readonly IOptions<AliExpressOptions> _options;
        private readonly IMapper _mapper;
        private readonly IAzureAliExpressOrderLogisticRedefiningRepository _aliExpressOrderLogisticRedefiningRepository;
        private readonly ITopClient _client;
        public AliExpressLogisticRedefiningService(ILogger<AliExpressLogisticRedefiningService> logger, 
            IOptions<AliExpressOptions> options, 
            IMapper mapper, 
            IAzureAliExpressOrderLogisticRedefiningRepository aliExpressOrderLogisticRedefiningRepository)
        {
            _options = options;
            _mapper = mapper;
            _aliExpressOrderLogisticRedefiningRepository = aliExpressOrderLogisticRedefiningRepository;
            _client = new DefaultTopClient(options.Value.HttpsEndPoint, options.Value.AppKey, options.Value.AppSecret, "Json");
        }

        public List<AliExpressOrderLogisticDTO> LogisticsRedefiningListLogisticsServiceRequest()
        {
            AliexpressLogisticsRedefiningListlogisticsserviceRequest req = new AliexpressLogisticsRedefiningListlogisticsserviceRequest();
            AliexpressLogisticsRedefiningListlogisticsserviceResponse rsp = _client.Execute(req, _options.Value.AccessToken);
            var aliExpressLogisticsRedefiningResponseRoot = JsonConvert.DeserializeObject<AliExpressLogisticsRedefiningResponseRoot>(rsp.Body);
            return aliExpressLogisticsRedefiningResponseRoot?.AliExpressLogisticsResponse
                .AliExpressLogisticsResponseResult.AliExpressOrderLogisticDtos;
        }

        public async Task ProcessLogisticRedefining(List<AliExpressOrderLogisticDTO> aliExpressOrderLogisticDtos)
        {
            var aliExpressOrderLogistics = _mapper.Map<List<AliExpressOrderLogisticDTO>, List<AliExpressOrderLogisticRedefining>>(aliExpressOrderLogisticDtos);
            var aliExpressOrderInDb = await _aliExpressOrderLogisticRedefiningRepository.GetInAsync("logistic_company", new { logistic_company = aliExpressOrderLogistics.Select(x=>x.LogisticCompany)});
            var newOrderLogistics = aliExpressOrderLogistics.Where(orderLogistic => aliExpressOrderInDb.All(orderDb => orderDb.LogisticCompany != orderLogistic.LogisticCompany));
            if (newOrderLogistics.Any())
            {

                var insertOrder = new AliExpressOrderLogisticRedefining().InsertString("dbo.order_redefining");
                //join and select
                await _aliExpressOrderLogisticRedefiningRepository.InsertAsync(insertOrder, newOrderLogistics.Select(x => new
                {
                    recommend_order = x.RecommendOrder,
                    tracking_no_regex = x.TrackingNoRegex,
                    min_process_day = x.MinProcessDay,
                    max_process_day = x.MaxProcessDay,
                    logistic_company = x.LogisticCompany,
                    display_name = x.DisplayName,
                    service_name = x.ServiceName
                }));
            }
        }

        public async Task<AliExpressOrderLogisticRedefining> GetRedefining(long orderId)
        {
            var orderRedefining = await _aliExpressOrderLogisticRedefiningRepository.GetAsync("select * from dbo.order_redefining where order_id = @order_id",new { order_id = orderId });
            return orderRedefining.FirstOrDefault();
        }
        
    }
}
