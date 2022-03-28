using Newtonsoft.Json;

namespace YapartMarket.React.ViewModels.Goods
{
    public class OrderItemViewModel
    {
        [JsonProperty("itemIndex")]
        public string ItemIndex { get; set; }
        [JsonProperty("offerId")]
        public string OfferId { get; set; }
    }
}