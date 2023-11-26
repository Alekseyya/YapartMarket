using YapartMarket.Core.Data.Interfaces.Azure;
using YapartMarket.Core.Models.Azure;

namespace YapartMarket.Data.Implementation.Azure
{
    public sealed class CategoryRepository : AzureGenericRepository<Category>, ICategoryRepository
    {
        public CategoryRepository(string tableName, string connectionString) : base(tableName, connectionString)
        {
        }
    }
}
