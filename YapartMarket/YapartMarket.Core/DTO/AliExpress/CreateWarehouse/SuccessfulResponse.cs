using Newtonsoft.Json;

namespace YapartMarket.React.ViewModels.AliExpress
{
    public class SuccessfulResponse
    {
        [JsonProperty("cainiao_global_logistic_order_create_response")]
        public CainiaoGlobalLogisticOrderCreateResponse? cainiao_global_logistic_order_create_response { get; set; }
    }
    public class CainiaoGlobalLogisticOrderCreateResponse
    {
        [JsonProperty("result")]
        public Result? result { get; set; }
        [JsonProperty("request_id")]
        public string? request_id { get; set; }
    }

    public class Result
    {
        [JsonProperty("logistics_order_id")]
        public int logistics_order_id { get; set; }
    }
}
