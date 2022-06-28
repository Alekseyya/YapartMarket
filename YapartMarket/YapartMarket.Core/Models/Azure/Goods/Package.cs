﻿using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace YapartMarket.Core.Models.Azure.Goods
{
    public sealed class Package
    {
        [Column("id")]
        public int Id { get; set; }
        [Column("order_id")]
        public int OrderId { get; set; }
        [Column("shipment_id")]
        public string ShipmentId { get; set; }
        [Column("create")]
        public DateTime Create { get; set; }
    }
}
