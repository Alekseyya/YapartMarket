using System.Collections.Generic;

namespace YapartMarket.WebApi.ViewModel.Goods.Cancel
{
    public class Data
    {
        public List<Shipment>? shipments { get; set; }
        public int merchantId { get; set; }
    }

    public class Item
    {
        public string? itemIndex { get; set; }
        public string? goodsId { get; set; }
        public string? offerId { get; set; }
    }

    public class Meta
    {
    }

    public class Cancel
    {
        public Meta? meta { get; set; }
        public Data? data { get; set; }
    }

    public class Shipment
    {
        public string? shipmentId { get; set; }
        public List<Item>? items { get; set; }
        public string? fulfillmentMethod { get; set; }
    }
}
