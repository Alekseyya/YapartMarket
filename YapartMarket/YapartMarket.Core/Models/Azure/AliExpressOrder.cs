using System;
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
        public string SellerSignerFullName { get; set; }
        [Column("seller_login_id")]
        public string SellerLoginId { get; set; }
        /// <summary>
        /// Номер заказа AliExpress
        /// </summary>
        [Column("order_id")]
        public long OrderId { get; set; }
        /// <summary>
        /// Логистический статус
        /// </summary>
        [Column("logistics_status")]
        public LogisticsStatus LogisticsStatus { get; set; }
        [Column("biz_type")]
        public BizType BizType { get; set; }
        [Column("gmt_pay_time")]
        public DateTime? GmtPayTime { get; set; }
        [Column("end_reason")]
        public string EndReason { get; set; }
        [Column("total_product_count")]
        public int TotalProductCount { get; set; }
        [Column("total_pay_amount")]
        public decimal TotalPayAmount { get; set; }
        [Column("order_status")]
        public OrderStatus OrderStatus { get; set; }
        [Column("gmt_update")]
        public DateTime? GmtUpdate { get; set; }
        [Column("gmt_create")]
        public DateTime? GmtCreate { get; set; }
        [Column("fund_status")]
        public FundStatus FundStatus { get; set; }
        [Column("frozen_status")]
        public FrozenStatus FrozenStatus { get; set; }
        [Column("created")]
        public DateTime Created { get; set; }
        [Column("updated")]
        public DateTime Updated { get; set; }
        public virtual ICollection<AliExpressOrderDetail> AliExpressOrderDetails { get; set; }
    }
}
