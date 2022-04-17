using System.ComponentModel.DataAnnotations.Schema;

namespace YapartMarket.Core.Models.Azure
{
    public sealed class ProductProperty
    {
        [Column("id")]
        public int Id { get; set; }
        [Column("product_id")]
        public long ProductId { get; set; }
        [Column("attr_name")]
        public string AttributeName { get; set; }
        [Column("attr_name_id")]
        public long AttributeNameId  { get; set; }
        [Column("attr_value")]
        public string AttributeValue { get; set; }
    }
}
