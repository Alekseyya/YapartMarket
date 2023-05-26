namespace YapartMarket.WebApi.Model.Goods
{
    public sealed class OrderItemViewModel
    {
        public long OfferId { get; set; }
        public string Sku { get; set; }
        public int Quantity { get; set; }
    }
}