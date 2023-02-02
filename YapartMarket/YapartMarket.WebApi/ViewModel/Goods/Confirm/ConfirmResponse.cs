using System.Text.Json.Serialization;

namespace YapartMarket.WebApi.ViewModel.Goods.Confirm
{
    /// <inheritdoc />
    public sealed class ConfirmResponse : SuccessResponse
    {
        [JsonPropertyName("data")]
        public Goods.Data? Data { get; set; }
        [JsonPropertyName("meta")]
        public Goods.Meta? Meta { get; set; }
    }
}
