using YapartMarket.Core.Data.Interfaces.Azure;
using YapartMarket.Core.Models.Azure;

namespace YapartMarket.Data.Implementation.Azure
{
    public class AzureAliExpressOrderDetailRepository : AzureGenericRepository<AliExpressOrderDetail>, IAzureAliExpressOrderDetailRepository
    {
        public AzureAliExpressOrderDetailRepository(string tableName, string connectionString) : base(tableName, connectionString)
        {
        }
    }
}
