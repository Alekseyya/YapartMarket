using YapartMarket.Core.BL;
using YapartMarket.Core.Data;
using YapartMarket.Core.Data.Interfaces;
using YapartMarket.Core.Models;

namespace YapartMarket.BL.Implementation
{
   public class CategoryService : GenericService<Category,int, ICategoryRepository>, ICategoryService
    {
        public CategoryService(IRepositoryFactory repositoryFactory) : base(repositoryFactory)
        {
        }
    }
}
