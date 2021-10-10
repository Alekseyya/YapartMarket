using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
            var aliExpressOrderLogistics = _mapper.Map<List<AliExpressOrderLogisticDTO>, List<AliExpressOrderLogistic>>(aliExpressOrderLogisticDtos);
            var aliExpressOrderInDb = await _aliExpressOrderLogisticRedefiningRepository.GetInAsync("order_id", new {order_id = aliExpressOrderLogistics.Select(x=>x.OrderId)});
            var newOrderLogistics = aliExpressOrderLogistics.Except(aliExpressOrderInDb);
            if (newOrderLogistics.Any())
            {
                //join and select
                _aliExpressOrderLogisticRedefiningRepository.InsertAsync(newOrderLogistics.Select(x => new
                {
                    order_id = x.OrderId,
                    recommend_order = x.RecommendOrder,
                    tracking_no_regex = x.TrackingNoRegex,
                    min_process_day = x.MinProcessDay,
                    logistics_company = x.LogisticCompany,
                    max_process_day = x.MaxProcessDay,
                    display_name = x.DisplayName,
                    service_name = x.ServiceName
                }));
            }
            
        }
        
    }
}
