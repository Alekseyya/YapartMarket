using YapartMarket.Core.DateStructures;

namespace YapartMarket.WebApi.ViewModel
{
    public sealed class AliExpressOrderDetail
    {
        public int ProductCount { get; init; }
        public long ProductId { get; init; }
        public string? ProductName { get; init; }
        public decimal ItemPrice { get; init; }
        public OrderStatus ShowStatus { get; init; }
        public int GoodsPrepareDays { get; init; }
        public decimal TotalProductAmount { get; init; }
    }
}
