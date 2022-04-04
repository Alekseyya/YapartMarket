using YapartMarket.Core.Data.Interfaces.Azure;
using YapartMarket.Core.Models.Azure;

namespace YapartMarket.Data.Implementation.Azure
{
    public class AliExpressLogisticOrderDetailRepository : AzureGenericRepository<AliExpressLogisticOrderDetail>, IAliExpressLogisticOrderDetailRepository
    {
        public AliExpressLogisticOrderDetailRepository(string tableName, string connectionString) : base(tableName, connectionString)
        {
        }
    }
}
