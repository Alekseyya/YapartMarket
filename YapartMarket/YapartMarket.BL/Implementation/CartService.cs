using YapartMarket.Core.BL;
using YapartMarket.Core.Data;

namespace YapartMarket.BL.Implementation
{
   public class CartService : RepositoryAwareServiceBase, ICartService
    {
        public CartService(IRepositoryFactory repositoryFactory) : base(repositoryFactory)
        {
        }
    }
}
