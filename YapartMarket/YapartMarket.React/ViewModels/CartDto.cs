using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace YapartMarket.React.ViewModels
{
    public class CartDto
    {
        [JsonPropertyName("cart")]
        public CartInfoDto Cart { get; set; }
    }

    public class CartInfoDto
    {
        [JsonPropertyName("currency")]
        public string Currency { get; set; }
        [JsonPropertyName("delivery")]
        public DeliveryDto Delivery { get; set; }
        [JsonPropertyName("items")]
        public List<CartItemDto> CartItems { get; set; }
    }

    public class CartItemDto
    {
        [JsonPropertyName("count")]
        public int Count { get; set; }
        [JsonPropertyName("feedId")]
        public Int64 FeedId { get; set; }
        [JsonPropertyName("offerId")]
        [Display(Name = "SKU")]
        public string OfferId { get; set; }
    }

    public enum Currency
    {
        RUB
    }
}
