using System.ComponentModel.DataAnnotations.Schema;

namespace YapartMarket.Core.Models.Azure.Goods
{
    public class GoodsOrder
    {
        [Column("id")]
        public int Id { get; set; }
        [Column("shipmentId")]
        public string? ShipmentId { get; set; }
    }
}
