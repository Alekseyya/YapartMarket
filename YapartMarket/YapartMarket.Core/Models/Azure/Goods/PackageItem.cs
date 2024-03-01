using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace YapartMarket.Core.Models.Azure.Goods
{
    public sealed class PackageItem
    {
        [Column("id")]
        public int Id { get; set; }
        [Column("item_index")]
        public int ItemIndex { get; set; }
        [Column("box_index")]
        public int BoxIndex { get; set; }
        [Column("box_code")]
        public string? BoxCode { get; set; }
        [Column("digital_marks")]
        public string? DigitalRemarks { get; set; }
        [Column("shipment_id")]
        public string? ShipmentId { get; set; }
        [Column("create")]
        public DateTime Create { get; set; }
    }
}
