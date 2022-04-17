using YapartMarket.Core.Data.Interfaces.Azure;
using YapartMarket.Core.Models.Azure;

namespace YapartMarket.Data.Implementation.Azure
{
    public class ProductPropertiesRepository : AzureGenericRepository<ProductProperty>, IProductPropertyRepository
    {
        public ProductPropertiesRepository(string tableName, string connectionString) : base(tableName, connectionString)
        {
        }
    }
}
