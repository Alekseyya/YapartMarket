using System.ComponentModel.DataAnnotations.Schema;

namespace YapartMarket.Core.Models.Azure
{
    public class GoodsOrderItem
    {
        [Column("id")]
        public int Id { get; set; }
        [Column("offerId")]
        public string OfferId { get; set; }
        [Column("orderId")]
        public int OrderId { get; set; }
        [Column("item_index")]
        public string ItemIndex { get; set; }
        [Column("reason_type")]
        public int ReasonType { get; set; }
    }
}
