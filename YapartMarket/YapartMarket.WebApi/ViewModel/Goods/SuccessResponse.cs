using System.Text.Json.Serialization;

namespace YapartMarket.WebApi.ViewModel.Goods
{
    public abstract class SuccessResponse
    {
        [JsonPropertyName("success")]
        public int Success { get; set; }
    }
}
