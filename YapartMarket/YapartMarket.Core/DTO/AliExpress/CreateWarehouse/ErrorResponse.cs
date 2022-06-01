using Newtonsoft.Json;

namespace YapartMarket.Core.DTO.AliExpress.CreateWarehouse
{
    public sealed class CainiaoGlobalLogisticOrderCreateResponse
    {
        [JsonProperty("error_info")]
        public ErrorInfo error_info { get; set; }
        [JsonProperty("request_id")]
        public string request_id { get; set; }
    }

    public sealed class ErrorInfo
    {
        [JsonProperty("error_code")]
        public string error_code { get; set; }
        [JsonProperty("error_msg")]
        public string error_msg { get; set; }
    }

    public sealed class ErrorResponse
    {
        [JsonProperty("cainiao_global_logistic_order_create_response")]
        public CainiaoGlobalLogisticOrderCreateResponse cainiao_global_logistic_order_create_response { get; set; }
    }
}
