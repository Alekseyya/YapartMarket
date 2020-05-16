using YapartMarket.Core.BL;
using YapartMarket.Core.Data;
using YapartMarket.Core.Data.Interfaces;
using YapartMarket.Core.Models;

namespace YapartMarket.BL.Implementation
{
    public class ProductModificationService : GenericService<ProductModification, int, IProductModificationRepository>, IProductModificationService
    {
        public ProductModificationService(IRepositoryFactory repositoryFactory) : base(repositoryFactory)
        {
        }
    }
}
