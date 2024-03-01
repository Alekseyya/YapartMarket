using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Dapper.Contrib.Extensions;
using YapartMarket.Core.DateStructures;

namespace YapartMarket.Core.Models.Azure
{
    public class AliExpressOrder
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Column("seller_signer_fullname")]
        public string? BuyerName { get; set; }
        [Column("order_id")]
        public long OrderId { get; set; }
        [Column("order_status")]
        public OrderStatus OrderStatus { get; set; }
        [Column("gmt_pay_time")]
        public DateTime? PaidAt { get; set; }
        [Column("total_product_count")]
        public int TotalProductCount { get; set; }
        [Column("total_pay_amount")]
        public decimal TotalPayAmount { get; set; }
        [Column("gmt_update")]
        public DateTime? UpdateAt { get; set; }
        [Column("gmt_create")]
        public DateTime? CreateAt { get; set; }
        [Column("fund_status")]
        public PaymentStatus PaymentStatus { get; set; }
        public virtual IList<AliExpressOrderDetail>? AliExpressOrderDetails { get; set; }
    }
}
