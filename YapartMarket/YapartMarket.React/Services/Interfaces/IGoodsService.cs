using System.Collections.Generic;
using System.Threading.Tasks;
using YapartMarket.Core.DTO.Goods;
using YapartMarket.React.ViewModels.Goods;

namespace YapartMarket.React.Services.Interfaces
{
    public interface IGoodsService
    {
        Task<List<OrderItem>> GetOrders(OrderNewViewModel order);
        Task<bool> Confirm(string shipmentId, int orderId);
        Task<bool> Shipment(string shipmentId);
        Task<bool> Reject(string shipmentId, int orderId);
        Task<bool> Package(string shipmentId, int orderId);
        Task<int> SaveOrder(string shipmentId, List<OrderItem> orderDetails);
    }
}
