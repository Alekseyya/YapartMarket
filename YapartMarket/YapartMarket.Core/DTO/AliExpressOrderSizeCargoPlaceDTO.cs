using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace YapartMarket.Core.DTO
{
    public class AliExpressLogisticsRedefiningGetOnlineLogisticsServiceListByOrderIdResponseRoot
    {
        [JsonProperty("aliexpress_logistics_redefining_getonlinelogisticsservicelistbyorderid_response")]
        public AliExpressLogisticsRedefiningGetOnlineLogisticsServiceListByOrderIdResponse AliExpressLogisticsRedefiningGetOnlineLogisticsServiceListByOrderIdResponse { get; set; }
    }

    public class AliExpressLogisticsRedefiningGetOnlineLogisticsServiceListByOrderIdResponse
    {
        [JsonProperty("result_list")]
        public AliExpressLogisticsRedefiningGetOnlineLogisticsServiceListByOrderIdResult AliExpressLogisticsRedefiningGetOnlineLogisticsServiceListByOrderIdResult { get; set; }
    }

    public class AliExpressLogisticsRedefiningGetOnlineLogisticsServiceListByOrderIdResult
    {
        [JsonProperty("result")]
        public List<AliExpressOrderSizeCargoPlaceDTO> AliExpressOrderSizeCargoPlaceDTOs { get; set; }
    }

    public class AliExpressOrderSizeCargoPlaceDTO
    {
        [JsonProperty("warehouse_name")]
        public string WarehouseName { get; set; }
        [JsonProperty("logistics_service_name")]
        public string LogisticServiceName { get; set; }
        [JsonProperty("logistics_timeliness")]
        public string LogisticTimeLines { get; set; }
        [JsonProperty("logistics_service_id")]
        public string LogisticServiceId { get; set; }
        [JsonProperty("delivery_address")]
        public string DeliveryAddress { get; set; }
        [JsonProperty("express_logistics_service")]
        public bool ExpressLogisticsService { get; set; }
        [JsonProperty("trial_result")]
        public string TrialResult { get; set; }
        //todo не сериализуется весь json!!!!
    }
}
