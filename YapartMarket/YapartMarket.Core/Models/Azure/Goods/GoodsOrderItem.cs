using System.ComponentModel.DataAnnotations.Schema;

namespace YapartMarket.Core.Models.Azure.Goods
{
    public class GoodsOrderItem
    {
        [Column("Id")]
        public int Id { get; set; }
        [Column("offerId")]
        public string OfferId { get; set; }
        [Column("orderId")]
        public int OrderId { get; set; }
        [Column("itemIndex")]
        public string ItemIndex { get; set; }
        [Column("reasonType")]
        public int ReasonType { get; set; }
    }
}
