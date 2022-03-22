using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace YapartMarket.React.ViewModels.Goods
{
    public class OrderConfirmShipment
    {
        [JsonPropertyName("shipmentId")]
        public string ShipmentId { get; set; }
        [JsonPropertyName("orderCode")]
        public string OrderCode { get; set; }
        [JsonPropertyName("items")]
        public List<OrderConfirmItem> Items { get; set; }
    }
}