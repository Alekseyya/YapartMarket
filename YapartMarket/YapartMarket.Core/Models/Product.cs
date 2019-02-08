
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace YapartMarket.Core.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Article { get; set; }
        public string ShortArticle { get; set; }
        public string Descriptions { get; set; }
        public decimal Price { get; set; }
        public int DaysDelivery { get; set; }
        //cross price
        public decimal OldPrice { get; set; }
        //show to popular or not
        public bool Popular { get; set; }
        // additional characteristic
        public string Characteristic { get; set; }
        //for seo
        public string Brief { get; set; }
        //see on layout
        public bool Show { get; set; }
        //see in discount tab
        public bool Discount { get; set; }
        public string Keywords { get; set; }
        public bool RemoveMarketplace { get; set; }
        
        public int? BrandId { get; set; }
        public Brand Brand { get; set; }

        public int? CategoryId { get; set; }
        public Category Category { get; set; }

        public ICollection<Picture> Pictures { get; set; }
        public ICollection<ProductModification> ProductModifications { get; set; }

    }
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Article).IsRequired().HasMaxLength(50);
            builder.Property(x => x.Descriptions).IsRequired();
            builder.Property(x => x.Price).HasColumnType("decimal(10,2)");
            builder.HasMany(x => x.Pictures).WithOne(x => x.Product);
            builder.HasMany(x => x.ProductModifications).WithOne(x => x.Product);
        }
    }
}
