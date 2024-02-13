using System;
using System.Collections.Generic;

namespace YapartMarket.Core.Models.AliProduct
{
    public class Datum
    {
        public string? id { get; set; }
        public DateTime ali_created_at { get; set; }
        public DateTime ali_updated_at { get; set; }
        public string? category_id { get; set; }
        public string? currency_code { get; set; }
        public string? delivery_time { get; set; }
        public string? owner_member_id { get; set; }
        public string? owner_member_seq { get; set; }
        public string? freight_template_id { get; set; }
        public List<object>? group_ids { get; set; }
        public string? main_image_url { get; set; }
        public List<string>? main_image_urls { get; set; }
        public List<Sku>? sku { get; set; }
        public string? Subject { get; set; }
    }

    public class ProductResponse
    {
        public List<Datum>? data { get; set; }
        public object? error { get; set; }
    }

    public class Sku
    {
        public string? id { get; set; }
        public string? sku_id { get; set; }
        public string? code { get; set; }
        public string? price { get; set; }
        public string? discount_price { get; set; }
        public string? ipm_sku_stock { get; set; }
    }
}
