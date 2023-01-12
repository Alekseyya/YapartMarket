using System.Text.Json.Serialization;

namespace YapartMarket.WebApi.ViewModel.Goods
{
    public class OrderNewShipmentItem
    {
        [JsonPropertyName("goodsId")]
        public string GoodsId { get; set; }
        [JsonPropertyName("offerId")]
        public string OfferId { get; set; }
        [JsonPropertyName("itemIndex")]
        public string ItemIndex { get; set; }
        [JsonPropertyName("itemName")]
        public string ItemName { get; set; }
        [JsonPropertyName("finalPrice")]
        public int FinalPrice { get; set; }
        [JsonPropertyName("price")]
        public int Price { get; set; }
        [JsonPropertyName("quantity")]
        public int Quantity { get; set; }
    }
}