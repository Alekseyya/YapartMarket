using System.Collections.Generic;
using Newtonsoft.Json;

namespace YapartMarket.Core.DTO.AliExpress
{
    public sealed class CategoryRoot
    {
        [JsonProperty("aliexpress_category_redefining_getpostcategorybyid_response")]
        public CategoryResult? Result { get; set; }
    }

    public sealed class CategoryResult
    {
        [JsonProperty("result")]
        public CategoryList? CategoryList { get; set; }
        [JsonProperty("request_id")]
        public string? RequestId { get; set; }
    }
    public sealed class CategoryList
    {
        [JsonProperty("aeop_post_category_list")]
        public PostCategoryList? PostCategoryList { get; set; }
        [JsonProperty("success")]
        public bool Success { get; set; }
    }

    public sealed class PostCategoryList
    {
        [JsonProperty("aeop_post_category_dto")]
        public List<Category>? CategoryInfo { get; set; }
    }

    public class Category
    {
        [JsonProperty("features")]
        public string? Features { get; set; }
        [JsonProperty("id")]
        public long Id { get; set; }
        [JsonProperty("isleaf")]
        public bool IsLeaf { get; set; }
        [JsonProperty("level")]
        public int Level { get; set; }
        [JsonProperty("names")]
        public string? MultilanguageName { get; set; }
    }

    public sealed class CategoryThreeRootError
    {
        [JsonProperty("aliexpress_solution_seller_category_tree_query_response")]
        public CategoryThreeError? Response { get; set; }
    }

    public sealed class CategoryThreeError
    {
        [JsonProperty("is_success")]
        public bool IsSuccess { get; set; }
        [JsonProperty("request_id")]
        public string? RequestId { get; set; }
    }
}
