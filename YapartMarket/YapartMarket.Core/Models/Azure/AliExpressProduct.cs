using System.ComponentModel.DataAnnotations.Schema;

namespace YapartMarket.Core.Models.Azure
{
     public class AliExpressProduct
    {
        [Column("id")]
        public int Id { get; set; }
        [Column("productId")]
        public long? ProductId { get; set; }
        [Column("sku")]
        public string Sku { get; set; }
        [Column("created")]
        public string Created { get; set; }
        [Column("updatedAt")]
        public string UpdatedAt { get; set; }

        [Column("category_id")]
        public long? CategoryId { get; set; }
        [Column("currency_code")]
        public string CurrencyCode { get; set; }
        [Column("group_id")]
        public long? GroupId { get; set; }
        [Column("gross_weight")]
        public string GrossWeight { get; set; }
        [Column("package_height")]
        public int PackageHeight { get; set; }
        [Column("package_length")]
        public int PackageLength { get; set; }
        [Column("package_width")]
        public int PackageWidth { get; set; }
        [Column("product_price")]
        public decimal ProductPrice { get; set; }
        [Column("product_status_type")]
        public string ProductStatusType { get; set; }
        [Column("product_unit")]
        public long? ProductUnit { get; set; }
    }
}
