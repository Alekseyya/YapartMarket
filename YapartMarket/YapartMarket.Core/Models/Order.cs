using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace YapartMarket.Core.Models
{
    public class Order
    {
        public int Id { get; set; }
        
        public string ShippingMethod { get; set; }
        public string PaymentMethod { get; set; }

        public Guid ClientGuid { get; set; }
        public DateTime? CreationTime { get; set; }

        public string City { get; set; }
        public string Phone { get; set; }

        public DateTime? ShippedDate { get; set; }
        public string Comment { get; set; }


        public bool IsSent{ get; set; }
        public bool IsClosed { get; set; }

        public ICollection<OrderItem> OrderItems { get; set; }
        
    }
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.CreationTime);
            builder.Property(x => x.ShippedDate);
        }
    }
}
