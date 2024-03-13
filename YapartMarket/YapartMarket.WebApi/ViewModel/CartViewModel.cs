using System;
using System.Collections.Generic;
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
        [JsonPropertyName("paymentMethods")]
        public List<string> PaymentMethods { get; set; }
    }
    public class CartItemViewModel
    {
        [JsonPropertyName("feedId")]
        public long FeedId { get; set; }
        [JsonPropertyName("offerId")]
        public string OfferId { get; set; }
        [JsonPropertyName("count")]
        public int Count { get; set; }
    }

}
