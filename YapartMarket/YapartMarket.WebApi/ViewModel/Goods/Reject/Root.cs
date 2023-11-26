namespace YapartMarket.WebApi.ViewModel.Goods.Reject
{
    public class Data
    {
        public List<Shipment> shipments { get; set; }
        public Reason reason { get; set; }
        public string token { get; set; }
    }

    public class Item
    {
        public int itemIndex { get; set; }
        public string offerId { get; set; }
    }

    public class Reason
    {
        public string type { get; set; }
    }

    public class Root
    {
        public Data data { get; set; }
        public List<object> meta { get; set; }
    }

    public class Shipment
    {
        public string shipmentId { get; set; }
        public List<Item> items { get; set; }
    }
}
