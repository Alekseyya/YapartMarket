﻿using System.Collections.Generic;
using System.Threading.Tasks;
using YapartMarket.Core.DTO.Goods;
using YapartMarket.React.ViewModels.Goods;
using YapartMarket.React.ViewModels.Goods.Shipment;

namespace YapartMarket.React.Services.Interfaces
{
    public interface IGoodsService
    {
        Task<List<OrderDetailDto>> GetOrders(OrderNewViewModel order);
        Task<bool> Confirm(string shipmentId, int orderId);
        Task<bool> Reject(string shipmentId, int orderId);
        Task<OrderShippingRoot> Shipment(string shipmentId);
        Task<bool> Package(string shipmentId, int orderId);
        Task<int> SaveOrder(string shipmentId, List<OrderDetailDto> orderDetails);
    }
}
