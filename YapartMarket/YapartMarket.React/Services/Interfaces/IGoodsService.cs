using System.Collections.Generic;
using System.Threading.Tasks;
using YapartMarket.React.ViewModels.Goods;

namespace YapartMarket.React.Services.Interfaces
{
    public interface IGoodsService
    {
        void GetOrders(OrderNewViewModel order, out List<OrderNewShipmentItem> confirmOrders, out List<OrderNewShipmentItem> rejectOrders);
        Task<bool> Confirm(string shipmentId, int orderId);
        Task<bool> Reject(string shipmentId, int orderId);
        Task<int> SaveOrder(string shipmentId, List<OrderNewShipmentItem> confirmOrderItems, List<OrderNewShipmentItem> rejectOrderItems);
    }
}
