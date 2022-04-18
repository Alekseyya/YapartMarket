using System.ComponentModel.DataAnnotations.Schema;

namespace YapartMarket.Core.Models.Azure
{
    public sealed class Category
    {
        [Column("id")]
        public int Id { get; set; }
        [Column("category_id")]
        public int CategoryId { get; set; }
        [Column("children_category_id")]
        public int ChildrenCategoryId { get; set; }
        [Column("is_leaf_category")]
        public bool LeafCategory { get; set; }
        [Column("level")]
        public int Level { get; set; }
        [Column("ru_language_name")]
        public string RuName { get; set; }
        [Column("en_language_name")]
        public string EnName { get; set; }
        [Column("cn_language_name")]
        public string CnName { get; set; }
    }
}
