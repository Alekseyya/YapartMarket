using YapartMarket.Core.Models.Azure;

namespace YapartMarket.Core.Data.Interfaces.Azure
{
    public interface IAzureAliExpressOrderDetailRepository : IAzureQueriesGenericRepository<AliExpressOrderDetail>, IAzureCommandGenericRepository<AliExpressOrderDetail>
    {
    }
}