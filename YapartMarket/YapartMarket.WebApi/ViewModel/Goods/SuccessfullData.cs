using System.Text.Json.Serialization;

namespace YapartMarket.React.ViewModels.Goods;

public class SuccessfullData
{
    [JsonPropertyName("result")]
    public int Result { get; set; }
}