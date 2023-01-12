using System.Collections.Generic;
using Newtonsoft.Json;

namespace YapartMarket.WebApi.ViewModel.Goods.Shipment
{
    public class OrderShippingRoot
    {
        [JsonProperty("data")]
        public OrderShipping Shipping { get; set; }
        [JsonProperty("meta")]
        public Meta Meta { get; set; }
    }

    public class Meta
    {
    }

    public class OrderShipping
    {
        [JsonProperty("token")]
        public string Token { get; set; }
        [JsonProperty("shipments")]
        public List<OrderShipment> Shipments { get; set; }
    }

    public class OrderShipment
    {
        [JsonProperty("shipmentId")]
        public string ShipmentId { get; set; }
        [JsonProperty("boxes")]
        public List<Box> Boxes { get; set; }
        [JsonProperty("shipping")]
        public Shipping Shipping { get; set; }
    }

    public class Shipping
    {
        [JsonProperty("shippingDate")]
        public string ShippingDate { get; set; }
    }

    public class Box
    {
        [JsonProperty("boxIndex")]
        public int BoxIndex { get; set; }
        [JsonProperty("boxCode")]
        public string BoxCode { get; set; }
    }
}
