using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace YapartMarket.Core.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
        //true sort on window
        public int Sort { get; set; }
        public string EnglishName { get; set; }

        public bool Show { get; set; }

        public int? SectionId { get; set; }
        public Section Section { get; set; }

        public ICollection<Product> Products { get; set; }
    }
    public class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Name).IsRequired();
            builder.HasMany(x => x.Products).WithOne(x=>x.Category);
        }
    }
}