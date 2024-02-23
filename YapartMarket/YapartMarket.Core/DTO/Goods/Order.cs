using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace YapartMarket.Core.DTO.Goods
{
    public sealed class Order
    {
        [Column("id")]
        public Guid Id { get; set; }
        [Column("shipmentId")]
        public string? ShipmentId { get; set; }
        [Column("shipmentDate")]
        public DateTime ShipmentDate { get; set; }
        public ICollection<OrderItem>? OrderDetails { get; set; }
    }
}
