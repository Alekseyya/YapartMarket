using YapartMarket.Core.BL;
using YapartMarket.Core.Data;

namespace YapartMarket.BL.Implementation
{
   public class OrderService: RepositoryAwareServiceBase, IOrderService
    {
        public OrderService(IRepositoryFactory repositoryFactory) : base(repositoryFactory)
        {
        }
    }
}
