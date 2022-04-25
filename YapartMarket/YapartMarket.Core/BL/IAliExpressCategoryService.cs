using System.Collections.Generic;
using System.Threading.Tasks;
using YapartMarket.Core.DTO.AliExpress;

namespace YapartMarket.Core.BL
{
    public interface IAliExpressCategoryService
    {
        Task ProcessUpdateCategories();
        Task<List<Category>> QueryCategoryAsync(long categoryId);
    }
}
