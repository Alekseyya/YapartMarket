using System.Threading.Tasks;

namespace YapartMarket.Core.BL
{
    public interface IAliExpressCategoryService
    {
        Task ProcessUpdateCategories();
        Task UpdateCategoryByProductId(long productId);
    }
}
