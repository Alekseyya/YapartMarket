using System;
using System.Collections.Generic;

namespace YapartMarket.WebApi.Model.Goods
{
    public sealed class OrderViewModel
    {
        public string ShipmentId { get; set; }
        public DateTime ShipmentDate { get; set; }
        public List<OrderItemViewModel> Items { get; set; }
    }
}
