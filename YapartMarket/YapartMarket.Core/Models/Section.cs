using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace YapartMarket.Core.Models
{
    /// <summary>
    /// Раздел
    /// </summary>
    public class Section
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool Show { get; set; }
        public int Sort { get; set; }

        public int? GroupId { get; set; }
        public Group Group { get; set; }

        public ICollection<Category> Categories { get; set; } 
    }

    public class SectionConfiguration : IEntityTypeConfiguration<Section>
    {
        public void Configure(EntityTypeBuilder<Section> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Name).IsRequired();
            builder.HasMany(x => x.Categories).WithOne(x => x.Section);
        }
    }
}