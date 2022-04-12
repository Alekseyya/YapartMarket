using System.Collections.Generic;
using Newtonsoft.Json;

namespace YapartMarket.Core.DTO.AliExpress
{
    public sealed class ProductInfoRoot
    {
        [JsonProperty("aliexpress_solution_product_info_get_response")]
        public ProductInfoResponse Response { get; set; }
    }

    public sealed class ProductInfoResponse
    {
        [JsonProperty("result")]
        public ProductInfoResult ProductInfoResult { get; set; }
    }

    public sealed class ProductInfoResult
    {
        [JsonProperty("aeop_ae_product_propertys")]
        public ProductInfoProperties ProductInfoProperties { get; set; }
        [JsonProperty("aeop_ae_product_s_k_us")]
        public ProductInfoSku ProductInfoSku { get; set; }
        [JsonProperty("category_id")]
        public long CategoryId { get; set; }
        [JsonProperty("currency_code")]
        public string CategoryCode { get; set; }
        [JsonProperty("gmt_create")]
        public string GmtCreate { get; set; }
        [JsonProperty("gmt_modified")]
        public string GmtModified { get; set; }
        [JsonProperty("group_id")]
        public long GroupId { get; set; }
        [JsonProperty("package_height")]
        public int PackageHeight { get; set; }
        [JsonProperty("package_length")]
        public int PackageLength { get; set; }
        [JsonProperty("package_width")]
        public int PackageWidth { get; set; }
        [JsonProperty("product_id")]
        public long ProductId { get; set; }
        [JsonProperty("product_price")]
        public string ProductPrice { get; set; }
        [JsonProperty("product_status_type")]
        public string ProductStatusType { get; set; }
        [JsonProperty("product_unit")]
        public long ProductUnit { get; set; }
    }

    public sealed class ProductInfoSku
    {
        [JsonProperty("global_aeop_ae_product_sku")]
        public List<GlobalProductSku> GlobalProductSkus { get; set; }
    }

    public class GlobalProductSku
    {
        [JsonProperty("aeop_s_k_u_property_list")]
        public SkuProperty Property { get; set; }
        [JsonProperty("barcode")]
        public string Barcode { get; set; }
        [JsonProperty("sku_code")]
        public string Code { get; set; }
        [JsonProperty("currency_code")]
        public string CurrencyCode { get; set; }
        [JsonProperty("sku_price")]
        public string Price { get; set; }
        [JsonProperty("sku_discount_price")]
        public string DiscountPrice { get; set; }
        [JsonProperty("sku_stock")]
        public bool Stock { get; set; }
    }

    public sealed class SkuProperty
    {
        [JsonProperty("global_aeop_sku_property")]
        public List<GlobalSkuProperty> GlobalSkuProperties { get; set; }
    }

    public class GlobalSkuProperty
    {
        [JsonProperty("sku_property_id")]
        public int Id { get; set; }
        [JsonProperty("property_value_definition_name")]
        public string DefinitionName { get; set; }
        [JsonProperty("property_value_id")]
        public int ValueId { get; set; }
    }

    public sealed class ProductInfoProperties
    {
        [JsonProperty("global_aeop_ae_product_property")]
        public List<GlobalProductProperty> GlobalProductProperties { get; set; }
    }

    public sealed class GlobalProductProperty
    {
        [JsonProperty("attr_name")]
        public string AttributeName { get; set; }
        [JsonProperty("attr_name_id")]
        public long AttributeNameId { get; set; }
        [JsonProperty("attr_value")]
        public string AttributeValue { get; set; }
    }
}
