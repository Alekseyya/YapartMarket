using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace YapartMarket.Core.Models.Azure.Goods
{
    public sealed class Package
    {
        [Column("Id")]
        public int Id { get; set; }
        [Column("orderId")]
        public int OrderId { get; set; }
        [Column("shipmentId")]
        public string? ShipmentId { get; set; }
        [Column("create")]
        public DateTime Create { get; set; }
    }
}
