using System.Text.Json.Serialization;

namespace YapartMarket.React.ViewModels.Goods
{
    public sealed class SuccessfulResponse
    {
        [JsonPropertyName("data")]
        public SuccessfullData Data { get; set; }
        [JsonPropertyName("meta")]
        public SuccessfullMeta Meta { get; set; }
        [JsonPropertyName("success")]
        public int Success { get; set; }
    }
}
