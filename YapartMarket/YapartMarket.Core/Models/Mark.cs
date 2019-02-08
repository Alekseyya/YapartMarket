using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace YapartMarket.Core.Models
{
    public class Mark
    {
        public int Id { get; set; }
        public string Name { get; set; }
        //show on main window
        public bool Show { get; set; }

        public ICollection<Picture> Pictures { get; set; }

        public ICollection<Model> Models { get; set; }
    }

    public class MarkConfiguration : IEntityTypeConfiguration<Mark>
    {
        public void Configure(EntityTypeBuilder<Mark> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Name).IsRequired();
            builder.HasMany(x => x.Models).WithOne(x=>x.Mark);
            builder.HasMany(x => x.Pictures).WithOne(x => x.Mark);
        }
    }
}
