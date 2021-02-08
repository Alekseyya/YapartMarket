using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

        public async Task<List<Brand>> GetBrands()
        {
            return await base.Get();
        }

        public async Task<Brand> CreateBrandAsync(Brand brand)
        {
            await base.AddAsync(brand);
            return base.Get(x => x.Where(br => br.Name == brand.Name).ToList()).FirstOrDefault();
        }

        public Task<Brand> GetBrandAsync(int id)
        {
            return Task.FromResult(base.GetById(id));
        }
    }
}
