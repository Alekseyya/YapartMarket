using YapartMarket.Core.BL;
using YapartMarket.Core.Data;
using YapartMarket.Core.Data.Interfaces;
using YapartMarket.Core.Models;

namespace YapartMarket.BL.Implementation
{
    public class CartLineService : GenericService<CartLine,int, ICartLineRepository>, ICartLineService
    {
        public CartLineService(IRepositoryFactory repositoryFactory) : base(repositoryFactory)
        {
        }
    }
}
