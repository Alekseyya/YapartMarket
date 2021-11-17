using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace YapartMarket.Core.Models.Azure
{
   public class AliExpressLogisticOrderDetail
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Column("order_id")]
        public long OrderId { get; set; }
        [Column("logistic_order_id")]
        public long LogisticOrderId { get; set; }
        [Column("out_order_code")]
        public string OutOrderCode { get; set; }
    }
}
