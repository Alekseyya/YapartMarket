using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace YapartMarket.React.ViewModels.Goods
{
    public class OrderNewShipment
    {
        [JsonPropertyName("shipmentId")]
        public string ShipmentId { get; set; }
        [JsonPropertyName("items")]
        public List<OrderNewShipmentItem> Items { get; set; }
    }
}