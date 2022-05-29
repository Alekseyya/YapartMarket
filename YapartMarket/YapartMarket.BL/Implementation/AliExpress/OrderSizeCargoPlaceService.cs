﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
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

namespace YapartMarket.BL.Implementation.AliExpress
{
    public class OrderSizeCargoPlaceService : IOrderSizeCargoPlaceService
    {
        private readonly IOptions<AliExpressOptions> _options;
        private readonly IMapper _mapper;
        private readonly IAliExpressOrderSizeCargoPlaceRepository _aliExpressOrderSizeCargoPlaceRepository;
        private ITopClient _client;
        public OrderSizeCargoPlaceService(IOptions<AliExpressOptions> options, 
            IMapper mapper,
            IAliExpressOrderSizeCargoPlaceRepository aliExpressOrderSizeCargoPlaceRepository)
        {
            _options = options;
            _mapper = mapper;
            _aliExpressOrderSizeCargoPlaceRepository = aliExpressOrderSizeCargoPlaceRepository;
            _client = new DefaultTopClient(options.Value.HttpsEndPoint, options.Value.AppKey, options.Value.AppSecret, "Json");
        }

        public List<AliExpressOrderSizeCargoPlaceDTO> GetRequest(long orderId)
        {
            AliexpressLogisticsRedefiningGetonlinelogisticsservicelistbyorderidRequest req = new AliexpressLogisticsRedefiningGetonlinelogisticsservicelistbyorderidRequest();
            req.OrderId = orderId;
            req.Locale = "ru_RU";
            AliexpressLogisticsRedefiningGetonlinelogisticsservicelistbyorderidResponse rsp = _client.Execute(req, _options.Value.AccessToken);
            var aliExpressOrderSizeCargoPlaceDTOs = JsonConvert
                .DeserializeObject<AliExpressLogisticsRedefiningGetOnlineLogisticsServiceListByOrderIdResponseRoot>
                    (rsp.Body)?.AliExpressLogisticsRedefiningGetOnlineLogisticsServiceListByOrderIdResponse
                .AliExpressLogisticsRedefiningGetOnlineLogisticsServiceListByOrderIdResult
                .AliExpressOrderSizeCargoPlaceDTOs;
            return aliExpressOrderSizeCargoPlaceDTOs;
        }

        public string CreateLogisticsServicesId(List<AliExpressOrderSizeCargoPlaceDTO> orderSizeCargoPlaces)
        {
            var result = string.Empty;
            if (orderSizeCargoPlaces.Any())
            {
                var logisticServices = orderSizeCargoPlaces.Select(x => x.LogisticServiceId).ToList();
                result = string.Join(";", logisticServices);
            }
            return result;
        }
        public async Task ProcessWriteOrderSizeCargoPlace(List<AliExpressOrderSizeCargoPlaceDTO> aliExpressOrderSize)
        {
            var aliExpressOrderSizeCargoPlaces = _mapper.Map<List<AliExpressOrderSizeCargoPlaceDTO>, List<AliExpressExpressOrderSizeCargoPlace>>(aliExpressOrderSize);
            var aliExpressOrderSizeInDb = await _aliExpressOrderSizeCargoPlaceRepository.GetInAsync("order_id", new { order_id = aliExpressOrderSizeCargoPlaces.Select(x => x.OrderId) });
            var newOrderSizeLogistics = aliExpressOrderSizeCargoPlaces.Except(aliExpressOrderSizeInDb);
            if (newOrderSizeLogistics.Any())
            {
                await _aliExpressOrderSizeCargoPlaceRepository.InsertAsync(newOrderSizeLogistics.Select(x => new
                {
                    order_id = x.OrderId,
                    warehouse_name = x.WarehouseName,
                    logistics_service_name = x.LogisticServiceName,
                    logistics_timeliness = x.LogisticTimeLines,
                    logistics_service_id = x.LogisticServiceId,
                    delivery_address = x.DeliveryAddress,
                    express_logistics_service = x.ExpressLogisticsService,
                    trial_result = x.TrialResult,
                    created = DateTime.Now,
                    updated = DateTime.Now
                 }));
            }

        }
    }
}
