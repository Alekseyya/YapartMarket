using YapartMarket.Core.BL;
using YapartMarket.Core.Data;

namespace YapartMarket.BL.Implementation
{
   public class BrandService : RepositoryAwareServiceBase, IBrandService
    {
        public BrandService(IRepositoryFactory repositoryFactory) : base(repositoryFactory)
        {
        }
    }
}
