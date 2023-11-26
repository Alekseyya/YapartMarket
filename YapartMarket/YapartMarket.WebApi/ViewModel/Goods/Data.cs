using System.Text.Json.Serialization;

namespace YapartMarket.WebApi.ViewModel.Goods;

public class Data
{
    [JsonPropertyName("result")]
    public int Result { get; set; }
}