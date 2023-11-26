using System.Collections.Generic;

namespace YapartMarket.WebApi.ViewModel.Goods.Packing
{
    public class Box
    {
        public int boxIndex { get; set; }
        public string boxCode { get; set; }
    }

    public class Data
    {
        public string token { get; set; }
        public List<Shipment> shipments { get; set; }
    }

    public class Item
    {
        public int itemIndex { get; set; }
        public List<Box> boxes { get; set; }
        public string digitalMark { get; set; }
    }

    public class Meta
    {
    }

    public class Root
    {
        public Data data { get; set; }
        public Meta meta { get; set; }
    }

    public class Shipment
    {
        public string shipmentId { get; set; }
        public string orderCode { get; set; }
        public List<Item> items { get; set; }
    }
}
