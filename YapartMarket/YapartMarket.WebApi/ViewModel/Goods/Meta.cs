
using System.Text.Json.Serialization;

namespace YapartMarket.WebApi.ViewModel.Goods
{
    public sealed class Meta
    {
        [JsonPropertyName("fromProxy")]
        public string? Proxy { get; set; }

        [JsonPropertyName("requestId")]
        public string? RequestId { get; set; }
    }
}