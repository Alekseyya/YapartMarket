using YapartMarket.Core.Models.Azure;

namespace YapartMarket.Core.Data.Interfaces.Azure
{
    public interface ICategoryRepository : IAzureQueriesGenericRepository<Category>, IAzureCommandGenericRepository<Category>
    {
    }
}
