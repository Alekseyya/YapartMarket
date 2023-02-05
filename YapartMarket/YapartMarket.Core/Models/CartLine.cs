using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace YapartMarket.Core.Models
{
   public class CartLine
    {
        public int Id { get; set; }

        public int CartId { get; set; }
        public Cart Cart { get; set; }

        public string Article { get; set; }
        public string Descriptions { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }  
    }

    public class CartLineConfiguration : IEntityTypeConfiguration<CartLine>
    {
        public void Configure(EntityTypeBuilder<CartLine> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Article).IsRequired();
            builder.Property(x => x.Price).IsRequired();
            builder.Property(x => x.Quantity).IsRequired();
        }
    }
}
