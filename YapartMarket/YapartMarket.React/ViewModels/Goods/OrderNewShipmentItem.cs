using System.Text.Json.Serialization;

namespace YapartMarket.React.ViewModels.Goods
{
    public class OrderNewShipmentItem
    {
        [JsonPropertyName("goodsId")]
        public string GoodsId { get; set; }
        [JsonPropertyName("offerId")]
        public string OfferId { get; set; }
        [JsonPropertyName("itemIndex")]
        public string ItemIndex { get; set; }
    }
}