using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using YapartMarket.Core.DateStructures;

namespace YapartMarket.React.ViewModels
{
    public class AliExpressOrderViewModel
    {
        public long OrderId { get; set; }
        public string LogisticsStatus { get; set; }
        public string BizType { get; set; }
        public DateTime? GmtPayTime { get; set; }
        public string EndReason { get; set; }
        public int TotalProductCount { get; set; }
        public decimal TotalPayAmount { get; set; }
        public string OrderStatus { get; set; }
        public DateTime? GmtUpdate { get; set; }
        public DateTime? GmtCreate { get; set; }
        public string FundStatus { get; set; }
        public string FrozenStatus { get; set; }
        public virtual ICollection<AliExpressOrderDetailViewModel> AliExpressOrderDetails { get; set; }
    }

    public class AliExpressOrderDetailViewModel
    {
        public string LogisticsServiceName { get; set; }
        public long OrderId { get; set; }
        public int ProductCount { get; set; }
        public long ProductId { get; set; }
        public string Sku { get; set; }
        public string ProductName { get; set; }
        public decimal ProductUnitPrice { get; set; }
        public string SendGoodsOperator { get; set; }
        public string ShowStatus { get; set; }
        public int GoodsPrepareTime { get; set; }
        public decimal TotalProductAmount { get; set; }
    }
}
