using System;
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
        [Column("total_count")]
        public int TotalCount { get; set; }
        [Column("pay_count")]
        public int PayCount { get; set; }
        [Column("order_status")]
        public string OrderStatus { get; set; }
        [Column("product_id")]
        public int ProductId { get; set; }
        [Column("gmt_update")]
        public DateTime GmtUpdate { get; set; }
        [Column("gmt_create")]
        public DateTime GmtCreate { get; set; }
        [Column("found_status")]
        public string FoundStatus { get; set; }
        [Column("frozen_status")]
        public string FrozenStatus { get; set; }
    }
}
