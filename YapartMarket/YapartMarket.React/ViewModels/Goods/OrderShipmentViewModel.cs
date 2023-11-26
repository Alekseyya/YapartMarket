using Newtonsoft.Json;
using System.Collections.Generic;

namespace YapartMarket.React.ViewModels.Goods
{
    public class OrderShipmentViewModel
    {
        [JsonProperty("shipmentId")]
        public string ShipmentId { get; set; }
        [JsonProperty("orderCode", NullValueHandling = NullValueHandling.Ignore)]
        public string OrderCode { get; set; }
        [JsonProperty("items")]
        public List<OrderItemViewModel> Items { get; set; }
    }
}