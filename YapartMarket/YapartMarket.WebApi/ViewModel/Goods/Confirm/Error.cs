using System.Text.Json.Serialization;

namespace YapartMarket.WebApi.ViewModel.Goods.Confirm;

public sealed class Error
{
    [JsonPropertyName("code")]
    public int? Code { get; set; }

    [JsonPropertyName("message")]
    public string? Message { get; set; }
}