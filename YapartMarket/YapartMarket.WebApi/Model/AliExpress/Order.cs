using System;
using System.Collections.Generic;

namespace YapartMarket.WebApi.Model.AliExpress
{
    sealed class Order
    {
        public long OrderId { get; init; }
        public string? LogisticsStatus { get; init; }
        public string? BizType { get; init; }
        public DateTime? GmtPayTime { get; init; }
        public string? EndReason { get; init; }
        public int TotalProductCount { get; init; }
        public decimal TotalPayAmount { get; init; }
        public string? OrderStatus { get; init; }
        public DateTime? GmtUpdate { get; init; }
        public DateTime? GmtCreate { get; init; }
        public string? FundStatus { get; init; }
        public string? FrozenStatus { get; init; }
        public IReadOnlyCollection<OrderDetail>? OrderDetails { get; init; }
    }
}
