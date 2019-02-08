using YapartMarket.Core.BL;
using YapartMarket.Core.Data;

namespace YapartMarket.BL.Implementation
{
   public class CategoryService : RepositoryAwareServiceBase, ICategoryService
    {
        public CategoryService(IRepositoryFactory repositoryFactory) : base(repositoryFactory)
        {
        }
    }
}
