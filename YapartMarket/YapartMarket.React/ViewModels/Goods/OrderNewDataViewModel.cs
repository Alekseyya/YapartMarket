using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace YapartMarket.React.ViewModels.Goods
{
    public class OrderNewDataViewModel
    {
        [JsonPropertyName("merchantId")]
        public int MerchantId { get; set; }
        [JsonPropertyName("shipments")]
        public List<OrderNewShipment> Shipments { get; set; }
    }
}