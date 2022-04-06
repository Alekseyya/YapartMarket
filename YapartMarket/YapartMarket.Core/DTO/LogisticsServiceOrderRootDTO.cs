using System.Collections.Generic;
using Newtonsoft.Json;

namespace YapartMarket.Core.DTO
{
    public sealed class LogisticsServiceOrderRootDTO
    {
        [JsonProperty("aliexpress_logistics_redefining_getonlinelogisticsservicelistbyorderid_response")]
        public LogisticsServiceOrderDTO LogisticsServiceOrderDto { get; set; }
    }

    public sealed class LogisticsServiceOrderDTO
    {
        [JsonProperty("result_list")] 
        public LogisticsServiceOrderResultListDTO LogisticsServiceOrderResultListDto { get; set; }
    }

    public sealed class LogisticsServiceOrderResultListDTO
    {
        [JsonProperty("result")]
        public List<LogisticsServiceOrderResultDTO> LogisticsServiceOrderResultDtos { get; set; }
    }

    public class LogisticsServiceOrderResultDTO
    {
        [JsonProperty("warehouse_name")]
        public string WarehouseName { get; set; }
        [JsonProperty("logistics_service_name")]
        public string LogisticsServiceName { get; set; }
        [JsonProperty("logistics_service_id")]
        public string LogisticsServiceId { get; set; }
        [JsonProperty("delivery_address")]
        public string DeliveryAddress { get; set; }
    }
}
