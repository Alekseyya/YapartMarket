using Microsoft.EntityFrameworkCore;
using System;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace YapartMarket.Core.Models
{
    public class OrderItem
    {
        public int Id { get; set; }

        public int? OrderId { get; set; }
        public Order Order { get; set; }
        public string Articul { get; set; }
        public string Description { get; set; }
        
        public decimal PriceWithDiscount { get; set; }
        
        public int Quantity { get; set; }
        public string Comment { get; set; }
        
        public bool IsClosed { get; set; }
        public string OrderStatus { get; set; }
        public string OrderStatusComment { get; set; }
        public Guid ClientGuid { get; set; }

        public DateTime? CreationTime { get; set; }
    }
    public class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
    {
        public void Configure(EntityTypeBuilder<OrderItem> builder)
        {
            builder.HasKey(x => x.Id);
            
            builder.Property(x => x.ClientGuid).IsRequired();
            builder.Property(x => x.CreationTime);
        }
    }
}
