using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace YapartMarket.Core.Models
{
    public class Brand
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public int? PictureId { get; set; }
        public Picture Picture { get; set; }

        public ICollection<Product> Products { get; set; }
    }

    public class BrandConfiguration : IEntityTypeConfiguration<Brand>
    {

        public void Configure(EntityTypeBuilder<Brand> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Name).IsRequired();

            builder.HasMany(x => x.Products).WithOne(x=>x.Brand);
            builder.HasOne(x => x.Picture).WithOne(x => x.Brand).HasForeignKey<Picture>(pic=>pic.BrandId);
        }
    }
}