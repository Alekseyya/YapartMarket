namespace YapartMarket.WebApi.ViewModel.Goods
{
    public sealed class Lable
    {
        public string deliveryId { get; set; }
        public string region { get; set; }
        public string city { get; set; }
        public string address { get; set; }
        public string fullName { get; set; }
        public string merchantName { get; set; }
        public int merchantId { get; set; }
        public string shipmentId { get; set; }
        public DateTime shippingDate { get; set; }
        public string deliveryType { get; set; }
        public string labelText { get; set; }
    }
}