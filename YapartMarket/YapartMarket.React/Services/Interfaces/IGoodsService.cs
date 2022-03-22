using System.Collections.Generic;
using System.Threading.Tasks;
using YapartMarket.React.ViewModels.Goods;

namespace YapartMarket.React.Services.Interfaces
{
    public interface IGoodsService
    {
        void GetOrders(OrderNewViewModel order, out List<OrderNewShipmentItem> confirmOrders, out List<OrderNewShipmentItem> rejectOrders);
        Task Confirm(string shipmentId, List<OrderNewShipmentItem> confirmOrders);
    }
}
