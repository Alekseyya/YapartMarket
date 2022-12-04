using System;
using System.ComponentModel.DataAnnotations.Schema;
using Dapper.Contrib.Extensions;
using YapartMarket.Core.DateStructures;

namespace YapartMarket.Core.Models.Azure
{
    public class AliExpressOrderDetail
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Column("logistics_service_name")]
        public string LogisticsServiceName { get; set; }
        [Column("order_id")]
        public long OrderId { get; set; }
        [Column("product_count")]
        public int ProductCount { get; set; }
        [Column("product_id")]
        public long ProductId { get; set; }
        [Column("product_name")]
        public string ProductName { get; set; }
        [Column("product_unit_price")]
        public decimal ProductUnitPrice { get; set; }
        [Column("send_goods_operator")]
        public ShipperType SendGoodsOperator { get; set; }
        [Column("show_status")]
        public OrderStatus ShowStatus { get; set; }
        [Column("goods_prepare_time")]
        public int GoodsPrepareDays { get; set; }
        [Column("total_count_product_amount")]
        public decimal TotalProductAmount { get; set; }
        [Column("created")]
        public DateTime Created { get; set; }
        [Column("updated")]
        public DateTime Updated { get; set; }
        [Computed]
        public Product Product { get; set; }
    }
}
