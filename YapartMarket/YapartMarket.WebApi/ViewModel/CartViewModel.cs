using System.Text.Json.Serialization;

namespace YapartMarket.WebApi.ViewModel
{
    public class CartViewModel
    {
        [JsonPropertyName("cart")]
        public CartInfoViewModel Cart { get; set; }
    }

    public class CartInfoViewModel
    {
        [JsonPropertyName("items")]
        public List<CartItemViewModel> CartItems { get; set; }
    }

    public class CartItemViewModel
    {
        [JsonPropertyName("feedId")]
        public Int64 FeedId { get; set; }
        [JsonPropertyName("offerId")]
        public string OfferId { get; set; }
        [JsonPropertyName("count")]
        public int Count { get; set; }
        [JsonPropertyName("delivery")]
        public bool Delivery { get; set; }
    }

}
