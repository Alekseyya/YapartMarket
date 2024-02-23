using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace YapartMarket.Core.DTO.Yandex
{
    public sealed class SkuInfoDto
    {
        [JsonPropertyName("sku")]
        public string? Sku { get; set; }
        [JsonPropertyName("warehouseId")]
        public long WarehouseId { get; set; }
        [JsonPropertyName("items")]
        public List<ProductDto>? Items { get; set; }
    }
}