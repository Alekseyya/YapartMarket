using System.Text.Json.Serialization;

namespace YapartMarket.React.ViewModels
{
    public class OrderViewModel
    {
        [JsonPropertyName("order")]
        public OrderInfoViewModel OrderInfoViewModel { get; set; }
    }

    public class OrderInfoViewModel
    {
        [JsonPropertyName("accepted")]
        public bool Accepted { get; set; }
        [JsonPropertyName("id")]
        public string Id { get; set; }
        [JsonPropertyName("reason")]
        public string Reason { get; set; }
    }

    public enum Reason
    {
        OUT_OF_DATE
    }
}
