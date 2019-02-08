using System;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace YapartMarket.Core.Models
{
    public class Picture
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }
        public DateTime? UpdateTimestamp { get; set; }

        public int? ProductId { get; set; }
        public virtual Product Product { get; set; }

        public int? MarkId { get; set; }
        public Mark Mark { get; set; }

        public int? ModelId { get; set; }
        public Model Model { get; set; }

        public int? ModificationId { get; set; }
        public Modification Modification { get; set; }
        
        public int? BrandId { get; set; }
        public Brand Brand { get; set; }
    }
    public class PictureConfiguration : IEntityTypeConfiguration<Picture>
    {
        public void Configure(EntityTypeBuilder<Picture> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Name).IsRequired();
            builder.Property(x => x.Path).IsRequired();
        }
    }
}
