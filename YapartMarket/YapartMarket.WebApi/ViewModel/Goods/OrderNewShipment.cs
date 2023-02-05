using System.Text.Json.Serialization;

namespace YapartMarket.WebApi.ViewModel.Goods
{
    public class OrderNewShipment
    {
        [JsonPropertyName("shipmentId")]
        public string ShipmentId { get; set; }
        [JsonPropertyName("shipmentDate")]
        public string ShipmentDate { get; set; }
        [JsonPropertyName("items")]
        public List<OrderNewShipmentItem> Items { get; set; }
    }
}