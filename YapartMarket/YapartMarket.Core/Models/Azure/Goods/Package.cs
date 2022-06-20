using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace YapartMarket.Core.Models.Azure.Goods
{
    public sealed class Package
    {
        [Column("id")]
        public int Id { get; set; }
        [Column("item_index")]
        public int ItemIndex { get; set; }
        [Column("order_id")]
        public int OrderId { get; set; }
        [Column("box_index")]
        public int BoxIndex { get; set; }
        [Column("create")]
        public DateTime Create { get; set; }
    }
}
