using System.Text.Json.Serialization;
using YapartMarket.WebApi.ViewModel.Goods.Confirm;

namespace YapartMarket.WebApi.ViewModel.Goods.Packing
{
    public sealed class PackingResponse : SuccessResponse
    {
        [JsonPropertyName("meta")]
        public Goods.Meta Meta { get; set; }
        [JsonPropertyName("error")]
        public Error Error { get; set; }
    }
}
