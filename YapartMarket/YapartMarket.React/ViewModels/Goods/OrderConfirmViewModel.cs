using System.Text.Json.Serialization;

namespace YapartMarket.React.ViewModels.Goods
{
    public class OrderConfirmViewModel
    {
        [JsonPropertyName("data")]
        public OrderConfirmData Data { get; set; }
        [JsonPropertyName("meta")]
        public OrderConfirmMeta Meta { get; set; }
    }
}
