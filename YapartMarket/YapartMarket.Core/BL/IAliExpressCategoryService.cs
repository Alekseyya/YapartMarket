using System.Threading.Tasks;

namespace YapartMarket.Core.BL
{
    public interface IAliExpressCategoryService
    {
        Task ProcessUpdateCategoriesAsync();
        Task UpdateCategoryByProductIdAsync(long productId);
    }
}
