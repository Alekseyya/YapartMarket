using YapartMarket.Core.BL;
using YapartMarket.Core.Data;
using YapartMarket.Core.Data.Interfaces;
using YapartMarket.Core.Models;

namespace YapartMarket.BL.Implementation
{
   public class BrandService : GenericService<Brand, int, IBrandRepository>, IBrandService
    {
        public BrandService(IRepositoryFactory repositoryFactory) : base(repositoryFactory)
        {
        }
    }
}
