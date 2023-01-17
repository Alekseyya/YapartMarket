using YapartMarket.Core.DTO.Goods;
using YapartMarket.WebApi.ViewModel.Goods;
using YapartMarket.WebApi.ViewModel.Goods.Cancel;

namespace YapartMarket.WebApi.Services.Interfaces
{
    public interface IGoodsService
    {
        Task CancelAsync(Cancel cancelOrder);
        Task<Order?> GetOrderAsync(OrderNewViewModel orderViewModel);
        Task ProcessConfirmOrRejectAsync(string? shipmentId);
        Task SaveOrderAsync(OrderNewViewModel orderViewModel);
    }
}
