using YapartMarket.Core.BL;
using YapartMarket.Core.Data;
using YapartMarket.Core.Data.Interfaces;
using YapartMarket.Core.Models;

namespace YapartMarket.BL.Implementation
{
   public class OrderItemService : GenericService<OrderItem,int, IOrderItemRepository>, IOrderItemService
    {
        public OrderItemService(IRepositoryFactory repositoryFactory) : base(repositoryFactory)
        {
        }
    }
}
