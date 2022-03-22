using System.Text.Json.Serialization;

namespace YapartMarket.React.ViewModels.Goods
{
    public class OrderConfirmItem
    {
        [JsonPropertyName("itemIndex")]
        public int ItemIndex { get; set; }
        [JsonPropertyName("offerId")]
        public string OfferId { get; set; }
    }
}