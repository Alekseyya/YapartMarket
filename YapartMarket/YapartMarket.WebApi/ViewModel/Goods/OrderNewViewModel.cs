using System.Text.Json.Serialization;

namespace YapartMarket.WebApi.ViewModel.Goods;

public class OrderNewViewModel
{
    [JsonPropertyName("data")]
    public OrderNewDataViewModel? OrderNewDataViewModel { get; set; }
}
