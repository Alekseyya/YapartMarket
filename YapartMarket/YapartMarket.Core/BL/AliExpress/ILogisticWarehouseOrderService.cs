using System.Threading.Tasks;

namespace YapartMarket.Core.BL.AliExpress
{
    public interface ILogisticWarehouseOrderService
    {
        Task CreateOrderAsync(long orderId);
    }
}
