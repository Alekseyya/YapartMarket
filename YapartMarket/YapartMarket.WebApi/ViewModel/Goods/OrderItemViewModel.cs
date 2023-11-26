using Newtonsoft.Json;
using System.Collections.Generic;

namespace YapartMarket.WebApi.ViewModel.Goods
{
    public class OrderItemViewModel
    {
        [JsonProperty("itemIndex")]
        public string ItemIndex { get; set; }
        [JsonProperty("offerId", NullValueHandling = NullValueHandling.Ignore)]
        public string OfferId { get; set; }
        [JsonProperty("boxes", NullValueHandling = NullValueHandling.Ignore)]
        public List<OrderBox> OrderBox { get; set; }
        [JsonProperty("digitalMark", NullValueHandling = NullValueHandling.Ignore)]
        public string DigitalMark { get; set; }
        
    }

    public class OrderBox
    {
        [JsonProperty("boxIndex")]
        public int BoxIndex { get; set; }
        [JsonProperty("boxCode")]
        public string BoxCode { get; set; }
    }
}