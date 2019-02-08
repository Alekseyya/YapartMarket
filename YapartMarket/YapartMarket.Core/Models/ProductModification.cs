using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace YapartMarket.Core.Models
{
   public class ProductModification
    {
        public int Id { get; set; }
        public int? ProductId { get; set; }
        public Product Product { get; set; }

        public int? ModificationId { get; set; }
        public Modification Modification { get; set; }
    }

    public class ProductModificationConfiguration : IEntityTypeConfiguration<ProductModification>
    {
        public void Configure(EntityTypeBuilder<ProductModification> builder)
        {
            builder.HasKey(x => new { x.ProductId, x.ModificationId });
        }
    }
}
