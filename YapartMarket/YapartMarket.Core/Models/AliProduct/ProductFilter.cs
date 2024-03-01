using System.Collections.Generic;

namespace YapartMarket.Core.Models.AliProduct
{
    public class Filter
    {
        public SearchContent? search_content { get; set; }
    }

    public class ProductFilter
    {
        public Filter? filter { get; set; }
        public int limit { get; set; }
    }

    public class SearchContent
    {
        public List<string>? content_values { get; set; }
        public string? content_type { get; set; }
    }
}
