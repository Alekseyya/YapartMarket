using System.Collections.Generic;
using Newtonsoft.Json;

namespace YapartMarket.Core.DTO.AliExpress
{
    public sealed class CategoryThreeRoot
    {
        [JsonProperty("aliexpress_solution_seller_category_tree_query_response")]
        public CategoryThreeResponse Response { get; set; }
    }

    public sealed class CategoryThreeResponse
    {
        [JsonProperty("children_category_list")]
        public ChildrenCategoryList ChildrenCategoryList { get; set; }
    }

    public sealed class ChildrenCategoryList
    {
        [JsonProperty("category_info")]
        public List<CategoryInfo> CategoryInfo { get; set; }
    }

    public class CategoryInfo
    {
        [JsonProperty("children_category_id")]
        public int ChildrenCategoryId { get; set; }
        [JsonProperty("is_leaf_category")]
        public bool LeafCategory { get; set; }
        [JsonProperty("multi_language_names")]
        public string MultilanguageName { get; set; }
        [JsonProperty("level")]
        public int Level { get; set; }
    }
}
