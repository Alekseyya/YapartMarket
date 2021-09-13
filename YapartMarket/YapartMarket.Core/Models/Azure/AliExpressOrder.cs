using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace YapartMarket.Core.Models.Azure
{
    public class AliExpressOrder
    {
        [Column("productId")]
        public int Id { get; set; }
        [Column("seller_signer_fullname")]
        public string SellerSignerFullname { get; set; }
        [Column("seller_login_in")]
        public string SellerLoginIn { get; set; }
        [Column("order_id")]
        public int OrderId { get; set; }
        [Column("time_stamp")]
        public DateTime TimeStamp { get; set; }
        [Column("total_product_count")]
        public int TotalCount { get; set; }
        [Column("total_pay_amount")]
        public int TotalPayAmount { get; set; }
        [Column("order_status")]
        public string OrderStatus { get; set; }
        [Column("gmt_update")]
        public DateTime GmtUpdate { get; set; }
        [Column("gmt_create")]
        public DateTime GmtCreate { get; set; }
        [Column("found_status")]
        public string FoundStatus { get; set; }
        [Column("frozen_status")]
        public string FrozenStatus { get; set; }
        public virtual ICollection<AliExpressOrderDetail> AliExpressOrderDetails { get; set; }
    }
}
