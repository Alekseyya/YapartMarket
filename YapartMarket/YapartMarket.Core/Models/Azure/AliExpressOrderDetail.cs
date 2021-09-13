using System.ComponentModel.DataAnnotations.Schema;
using YapartMarket.Core.DateStructures;

namespace YapartMarket.Core.Models.Azure
{
    public class AliExpressOrderDetail
    {
        [Column("id")]
        public int Id { get; set; }
        [Column("logistics_service_name")]
        public string LogisticsServiceName { get; set; }
        [Column("ali_order_id")]
        public long OrderId { get; set; }
        [Column("product_count")]
        public int ProductCount { get; set; }
        [Column("product_id")]
        public string ProductId { get; set; }
        [Column("product_name")]
        public string ProductName { get; set; }
        [Column("product_unit_price")]
        public double ProductUnitPrice { get; set; }
        [Column("send_goods_operator")]
        public ShipperType SendGoodsOperator { get; set; }
        [Column("show_status")]
        public OrderStatus ShowStatus { get; set; }
        [Column("goods_prepare_time")]
        public int GoodsPrepareTime { get; set; }
        [Column("total_product_amount")]
        public double TotalProductAmount { get; set; }
        public AliExpressOrder AliExpressOrder { get; set; }
    }
}
