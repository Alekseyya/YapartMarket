using YapartMarket.Core.BL;
using YapartMarket.Core.Data;
using YapartMarket.Core.Data.Interfaces;
using YapartMarket.Core.Models;

namespace YapartMarket.BL.Implementation
{
   public class CartService : GenericService<Cart, int, ICartRepository>, ICartService
    {
        public CartService(IRepositoryFactory repositoryFactory) : base(repositoryFactory)
        {
        }
    }
}
