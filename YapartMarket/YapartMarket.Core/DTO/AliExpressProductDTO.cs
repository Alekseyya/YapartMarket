using Newtonsoft.Json;

namespace YapartMarket.Core.DTO
{
    public class AliExpressProductDTO
    {
        
        public long ProductId { get; set; }
        public string Description { get; set; }
        /// <summary>
        /// Валюта
        /// </summary>
        [JsonProperty("currency_code")]
        public string CurrencyCode { get; set; }
        /// <summary>
        /// Количество товара
        /// </summary>
        [JsonProperty("ipm_sku_stock")]
        public int SkuStock { get; set; }
        /// <summary>
        /// SKU код
        /// </summary>
        [JsonProperty("sku_code")]
        public string SkuCode { get; set; }

        /// <summary>
        /// Цена со скидкой
        /// </summary>
        [JsonProperty("sku_discount_price")]
        public double SkuDiscountPrice { get; set; }
        /// <summary>
        /// Цена
        /// </summary>
        [JsonProperty("sku_price")]
        public double SkuPrice { get; set; }

        /// <summary>
        /// есть ли продукт
        /// </summary>
        [JsonProperty("sku_stock")]
        public bool IsSkuStock { get; set; }
    }
}
