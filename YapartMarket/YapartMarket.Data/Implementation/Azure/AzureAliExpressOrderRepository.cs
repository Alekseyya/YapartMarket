using YapartMarket.Core.Data.Interfaces.Azure;
using YapartMarket.Core.Models.Azure;

namespace YapartMarket.Data.Implementation.Azure
{
    public class AzureAliExpressOrderRepository : AzureGenericRepository<AliExpressOrder>, IAzureAliExpressOrderRepository
    {
        public AzureAliExpressOrderRepository(string tableName, string connectionString) : base(tableName, connectionString)
        {
        }
    }
}
