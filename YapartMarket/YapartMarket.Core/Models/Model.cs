using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace YapartMarket.Core.Models
{
    public class Model
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Years { get; set; }

        public int? MarkId { get; set; }
        public Mark Mark { get; set; }
        
        public ICollection<Picture> Pictures { get; set; }
        
        public ICollection<Modification> Modifications { get; set; }
    }
    public class ModelConfiguration : IEntityTypeConfiguration<Model>
    {
        public void Configure(EntityTypeBuilder<Model> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Name).IsRequired();
            builder.HasMany(x => x.Modifications).WithOne(x=>x.Model);
            builder.HasMany(x => x.Pictures).WithOne(x => x.Model);
        }
    }
}
