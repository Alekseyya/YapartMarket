﻿using Newtonsoft.Json;
using System.Collections.Generic;

namespace YapartMarket.WebApi.ViewModel.Goods
{
    public class OrderShipmentDataViewModel
    {
        [JsonProperty("token")]
        public string? Token { get; set; }
        [JsonProperty("shipments")]
        public List<OrderShipmentViewModel>? Shipments { get; set; }
    }
}