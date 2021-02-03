using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace YapartMarket.Core.Models
{
    /// <summary>
    /// Группа
    /// </summary>
    public class Group
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
    public class GroupConfiguration : IEntityTypeConfiguration<Group>
    {
        public void Configure(EntityTypeBuilder<Group> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Name).IsRequired();
        }
    }
}