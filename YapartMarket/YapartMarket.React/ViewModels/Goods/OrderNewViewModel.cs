using System.Text.Json.Serialization;

namespace YapartMarket.React.ViewModels.Goods
{
    public class OrderNewViewModel
    {
        [JsonPropertyName("data")]
        public OrderNewDataViewModel OrderNewDataViewModel { get; set; }
    }
}
