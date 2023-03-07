using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace YapartMarket.Core.DTO.Goods
{
    public sealed class OrderItem
    {
        [Column("id")]
        public Guid Id { get; set; }
        [Column("orderId")]
        public Guid OrderId { get; set; }
        [Column("itemIndex")]
        public int ItemIndex { get; set; }
        [Column("goodsId")]
        public long GoodsId { get; set; }
        [Column("offerId")]
        public string OfferId { get; set; }
        [Column("itemName")]
        public string ItemName { get; set; }
        [Column("price")]
        public int Price { get; set; }
        [Column("finalPrice")]
        public int FinalPrice { get; set; }
        [Column("quantity")]
        public int Quantity { get; set; }
        [Column("taxRate")]
        public string TaxRate { get; set; }
        [Column("reservatedPerfomed")]
        public bool ReservationPerformed { get; set; }
        [Column("isDigitalMarkRequired")]
        public bool IsDigitalMarkRequired { get; set; }
    }
}
