using System.Text.Json.Serialization;

namespace YapartMarket.React.ViewModels.Goods
{
    public class OrderConfirmItem
    {
        [JsonPropertyName("itemIndex")]
        public string ItemIndex { get; set; }
        [JsonPropertyName("offerId")]
        public string OfferId { get; set; }
    }
}