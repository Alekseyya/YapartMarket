using Newtonsoft.Json;

namespace YapartMarket.WebApi.ViewModel.Goods
{
    public class OrderShipmentDataViewModel
    {
        [JsonProperty("token")]
        public string Token { get; set; }
        [JsonProperty("shipments")]
        public List<OrderShipmentViewModel> Shipments { get; set; }
    }
}