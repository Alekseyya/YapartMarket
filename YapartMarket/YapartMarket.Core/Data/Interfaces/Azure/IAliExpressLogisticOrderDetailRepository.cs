using YapartMarket.Core.Models.Azure;

namespace YapartMarket.Core.Data.Interfaces.Azure
{
    public interface IAliExpressLogisticOrderDetailRepository : IAzureQueriesGenericRepository<AliExpressLogisticOrderDetail>, IAzureCommandGenericRepository<AliExpressLogisticOrderDetail>
    {
    }
}
