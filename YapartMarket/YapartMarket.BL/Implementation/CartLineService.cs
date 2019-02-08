using YapartMarket.Core.BL;
using YapartMarket.Core.Data;

namespace YapartMarket.BL.Implementation
{
    public class CartLineService : RepositoryAwareServiceBase, ICartLineService
    {
        public CartLineService(IRepositoryFactory repositoryFactory) : base(repositoryFactory)
        {
        }
    }
}
