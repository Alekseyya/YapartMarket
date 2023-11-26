using System.Collections.Generic;
using Newtonsoft.Json;

namespace YapartMarket.Core.DTO
{
    public class AliExpressLogisticsRedefiningResponseRoot
    {
        [JsonProperty("aliexpress_logistics_redefining_listlogisticsservice_response")]
        public AliExpressLogisticsResponse AliExpressLogisticsResponse { get; set; }
    }

    public class AliExpressLogisticsResponse
    {
        [JsonProperty("result_list")]
        public AliExpressLogisticsResponseResult AliExpressLogisticsResponseResult { get; set; }
        [JsonProperty("error_desc")]
        public string ErrorDesc { get; set; }
    }

    public class AliExpressLogisticsResponseResult
    {
        [JsonProperty("aeop_logistics_service_result")]
        public List<AliExpressOrderLogisticDTO> AliExpressOrderLogisticDtos { get; set; }
    }

    public class AliExpressOrderLogisticDTO
    {
        [JsonProperty("recommend_order")]
        public long RecommendOrder { get; set; }
        [JsonProperty("tracking_no_regex")]
        public string TrackingNoRegex { get; set; }
        [JsonProperty("min_process_day")]
        public int MinProcessDay { get; set; }
        [JsonProperty("logistics_company")]
        public string LogisticCompany { get; set; }
        [JsonProperty("max_process_day")]
        public int MaxProcessDay { get; set; }
        [JsonProperty("display_name")]
        public string DisplayName { get; set; }
        [JsonProperty("service_name")]
        public string ServiceName { get; set; }
    }
}
