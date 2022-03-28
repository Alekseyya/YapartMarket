using Newtonsoft.Json;

namespace YapartMarket.React.ViewModels.Goods
{
    public class ReasonViewModel
    {
        [JsonProperty("type")]
        public string Type { get; set; }
    }
}