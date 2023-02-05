using System.Text.Json.Serialization;

namespace YapartMarket.WebApi.ViewModel.Goods
{
    public class OrderNewDataViewModel
    {
        [JsonPropertyName("merchantId")]
        public int MerchantId { get; set; }
        [JsonPropertyName("shipments")]
        public List<OrderNewShipment> Shipments { get; set; }
    }
}