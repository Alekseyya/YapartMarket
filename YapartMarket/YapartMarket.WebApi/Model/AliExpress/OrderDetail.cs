namespace YapartMarket.WebApi.Model.AliExpress
{
    sealed class OrderDetail
    {
        public string LogisticsServiceName { get; init; }
        public long OrderId { get; init; }
        public int ProductCount { get; init; }
        public long ProductId { get; init; }
        public string Sku { get; init; }
        public string ProductName { get; init; }
        public decimal ProductUnitPrice { get; init; }
        public string SendGoodsOperator { get; init; }
        public string ShowStatus { get; init; }
        public int GoodsPrepareTime { get; init; }
        public decimal TotalProductAmount { get; init; }
    }
}
