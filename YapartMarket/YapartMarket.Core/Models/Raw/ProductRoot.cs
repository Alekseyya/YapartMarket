using System.Collections.Generic;

namespace YapartMarket.Core.Models.Raw
{
    public class Product
    {
        public string? product_id { get; set; }
        public List<Sku>? skus { get; set; }
    }

    public class ProductRoot
    {
        public List<Product>? products { get; set; }
    }

    public class Sku
    {
        public string? sku_code { get; set; }
        public string? inventory { get; set; }
    }
}
