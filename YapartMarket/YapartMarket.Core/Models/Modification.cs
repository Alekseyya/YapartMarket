using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace YapartMarket.Core.Models
{
    public class Modification
    {
        public int Id{ get; set; }
        public string Name { get; set; }

        public int? ModelId { get; set; }
        public Model Model { get; set; }

        public string Years { get; set; }
        public int Sort { get; set; }
        public string Url { get; set; }

        public ICollection<Picture> Pictures { get; set; }

        public ICollection<ProductModification> ProductModifications { get; set; }
    }

    public class ModificationConfiguration : IEntityTypeConfiguration<Modification>
    {

        public void Configure(EntityTypeBuilder<Modification> builder)
        {
            builder.HasKey(x => x.Id);
            builder.HasMany(x => x.ProductModifications).WithOne(x => x.Modification);
            builder.HasMany(x => x.Pictures).WithOne(x => x.Modification);
        }
    }
}
