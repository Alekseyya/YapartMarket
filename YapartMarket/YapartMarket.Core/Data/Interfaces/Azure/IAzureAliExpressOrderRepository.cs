using YapartMarket.Core.Models.Azure;

namespace YapartMarket.Core.Data.Interfaces.Azure
{
    public interface IAzureAliExpressOrderRepository : IAzureQueriesGenericRepository<AliExpressOrder>, IAzureCommandGenericRepository<AliExpressOrder>
    {
    }
}
