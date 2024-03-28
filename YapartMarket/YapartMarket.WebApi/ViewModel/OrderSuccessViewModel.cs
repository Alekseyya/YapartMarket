using System.Text.Json.Serialization;

namespace YapartMarket.WebApi.ViewModel
{
    public class OrderSuccessViewModel
    {
        [JsonPropertyName("order")]
        public OrderInfoSuccessViewModel? OrderInfoViewModel { get; init; }
    }
    public class OrderFailViewModel
    {
        [JsonPropertyName("order")]
        public OrderInfoFailViewModel? OrderInfoViewModel { get; init; }
    }

    public abstract class OrderInfoViewModel
    {
        [JsonPropertyName("accepted")]
        public bool Accepted { get; init; }
    }
    public class OrderInfoSuccessViewModel : OrderInfoViewModel
    {
        [JsonPropertyName("id")]
        public string? Id { get; init; }
    }
    public class OrderInfoFailViewModel : OrderInfoViewModel
    {
        [JsonPropertyName("reason")]
        public string? Reason { get; init; }
    }

    public enum Reason
    {
        OUT_OF_DATE
    }
}
