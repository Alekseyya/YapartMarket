using System.Collections.Generic;

namespace YapartMarket.WebApi.ViewModel.Goods.Confirm
{
    public class Data
    {
        public string token { get; set; }
        public List<Shipment> shipments { get; set; }
    }

    public class Item
    {
        public int itemIndex { get; set; }
        public string offerId { get; set; }
    }

    public class Meta
    {
    }

    public class Confirm
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
