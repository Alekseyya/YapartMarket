using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace YapartMarket.Core.Models
{
    public class Cart
    {
        public int Id { get; set; }
        public Guid UserId { get; set; }
        public ICollection<CartLine> Lines { get; set; }

    }
    public class CartConfiguration : IEntityTypeConfiguration<Cart>
    {
        public void Configure(EntityTypeBuilder<Cart> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.UserId).IsRequired();
            builder.HasMany(x => x.Lines).WithOne(x => x.Cart);
        }
    }
}
