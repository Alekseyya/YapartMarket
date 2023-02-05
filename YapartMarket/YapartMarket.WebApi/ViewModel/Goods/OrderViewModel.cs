using Newtonsoft.Json;

namespace YapartMarket.WebApi.ViewModel.Goods
{
    public class OrderViewModel
    {
        [JsonProperty("data")]
        public OrderShipmentDataViewModel Data { get; set; }
        [JsonProperty("meta")]
        public OrderConfirmMeta Meta { get; set; }
        [JsonProperty("reason", NullValueHandling = NullValueHandling.Ignore)]

        public ReasonViewModel Reason { get; set; }
    }
}
