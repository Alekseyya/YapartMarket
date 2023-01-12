using System.Text.Json.Serialization;
using YapartMarket.React.ViewModels.Goods;

namespace YapartMarket.WebApi.ViewModel.Goods
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
