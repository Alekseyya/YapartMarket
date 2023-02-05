using YapartMarket.Core.DTO.Goods;
using YapartMarket.WebApi.ViewModel.Goods;
using YapartMarket.WebApi.ViewModel.Goods.Cancel;

namespace YapartMarket.WebApi.Services.Interfaces
{
    public interface IGoodsService
    {
        Task<SuccessResult> ProcessConfirmOrRejectAsync(string? shipmentId);
        Task<SuccessResult> CancelAsync(Cancel cancelOrder);
        Task<Order?> GetOrderAsync(OrderNewViewModel orderViewModel);
        Task SaveOrderAsync(OrderNewViewModel orderViewModel);
    }
}
