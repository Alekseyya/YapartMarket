using System;
using System.Collections.Generic;

namespace YapartMarket.WebApi.ViewModel
{
    public class AliExpressOrder
    {
        public string? BuyerName { get; set; }
        public long OrderId { get; set; }
        public string? OrderStatus { get; set; }
        public DateTime? PaidAt { get; set; }
        public int TotalProductCount { get; set; }
        public decimal TotalPayAmount { get; set; }
        public DateTime? UpdateAt { get; set; }
        public DateTime? CreateAt { get; set; }
        public string? PaymentStatus { get; set; }
        public virtual ICollection<AliExpressOrderDetail>? OrderDetails { get; set; }
    }
}
