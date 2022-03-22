using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace YapartMarket.React.ViewModels.Goods
{
    public class OrderConfirmData
    {
        [JsonPropertyName("token")]
        public string Token { get; set; }
        [JsonPropertyName("shipments")]
        public List<OrderConfirmShipment> Shipments { get; set; }
    }
}