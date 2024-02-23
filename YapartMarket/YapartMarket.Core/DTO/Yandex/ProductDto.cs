using System;
using System.Text.Json.Serialization;

namespace YapartMarket.Core.DTO.Yandex
{
    public sealed class ProductDto
    {
        [JsonPropertyName("type")]
        public string? Type { get; set; }
        [JsonPropertyName("count")]
        public Int64 Count { get; set; }
        [JsonPropertyName("updatedAt")]
        public string? UpdatedAt { get; set; }
    }
}