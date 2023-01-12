using YapartMarket.Core.DTO.Goods;
using YapartMarket.WebApi.ViewModel.Goods;

namespace YapartMarket.WebApi.Services.Interfaces
{
    public interface IGoodsService
    {
        Task<Order?> GetOrderAsync(OrderNewViewModel orderViewModel);
        Task<bool> Confirm(string shipmentId, int orderId);
        Task<bool> Shipment(string shipmentId);
        Task<bool> Reject(string shipmentId, int orderId);
        Task<bool> Package(string shipmentId, int orderId);
        Task SaveOrderAsync(OrderNewViewModel orderViewModel);
    }
}
