using Newtonsoft.Json;

namespace YapartMarket.Core.DTO.AliExpress
{
    public sealed class LanguageNames
    {
        [JsonProperty("ru")]
        public string? Ru { get; set; }
        [JsonProperty("en")]
        public string? En { get; set; }
        [JsonProperty("cn")]
        public string? Cn { get; set; }
    }
}
