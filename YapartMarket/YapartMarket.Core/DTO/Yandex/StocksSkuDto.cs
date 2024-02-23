using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace YapartMarket.Core.DTO.Yandex
{
    public sealed class StocksSkuDto
    {
        [JsonPropertyName("skus")]
        public List<SkuInfoDto>? Skus { get; set; }
    }
}