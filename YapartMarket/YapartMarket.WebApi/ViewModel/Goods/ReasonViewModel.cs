using Newtonsoft.Json;

namespace YapartMarket.WebApi.ViewModel.Goods
{
    public class ReasonViewModel
    {
        [JsonProperty("type")]
        public string Type { get; set; }
    }
}