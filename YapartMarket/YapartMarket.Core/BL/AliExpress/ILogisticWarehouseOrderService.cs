using System.Threading.Tasks;

namespace YapartMarket.Core.BL.AliExpress
{
    public interface ILogisticWarehouseOrderService
    {
        Task CreateOrderAsync(long orderId);
        Task CreateWarehouseAsync(long orderId);
        Task CreateWarehouseOrderAsync(long orderId);
    }
}
