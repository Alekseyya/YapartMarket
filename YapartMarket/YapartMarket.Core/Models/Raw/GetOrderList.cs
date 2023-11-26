namespace YapartMarket.Core.Models.Raw
{
    public sealed class GetOrderList
    {
        public int page_size { get; set; }
        public int page { get; set; }
        public string date_start { get; set; }
        public string date_end { get; set; }
    }
}
