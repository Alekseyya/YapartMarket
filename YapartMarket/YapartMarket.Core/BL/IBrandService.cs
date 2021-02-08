
using System.Collections.Generic;
using System.Threading.Tasks;
using YapartMarket.Core.Models;

namespace YapartMarket.Core.BL
{
   public interface IBrandService
   {
       Task<List<Brand>> GetBrands();
       Task<Brand> CreateBrandAsync(Brand brand);
       Task<Brand> GetBrandAsync(int id);
   }
}
