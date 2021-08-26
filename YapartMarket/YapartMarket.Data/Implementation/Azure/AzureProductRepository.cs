using YapartMarket.Core.Data.Interfaces.Azure;
using YapartMarket.Core.Models.Azure;

namespace YapartMarket.Data.Implementation.Azure
{
    public class AzureProductRepository : AzureGenericRepository<Product>,  IAzureProductRepository
    {
        public AzureProductRepository(string tableName, string connectionString) : base(tableName, connectionString)
        {
        }
    }
}
