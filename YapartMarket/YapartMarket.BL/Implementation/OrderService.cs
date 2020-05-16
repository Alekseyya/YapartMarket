using YapartMarket.Core.BL;
using YapartMarket.Core.Data;
using YapartMarket.Core.Data.Interfaces;
using YapartMarket.Core.Models;

namespace YapartMarket.BL.Implementation
{
   public class OrderService: GenericService<Order, int, IOrderRepository>, IOrderService
    {
        public OrderService(IRepositoryFactory repositoryFactory) : base(repositoryFactory)
        {
        }
    }
}
