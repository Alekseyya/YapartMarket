using Newtonsoft.Json;
using System.Text.Json.Serialization;
using YapartMarket.WebApi.ViewModel.Goods;

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
