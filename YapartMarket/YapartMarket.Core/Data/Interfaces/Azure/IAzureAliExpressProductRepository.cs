using YapartMarket.Core.Models.Azure;

namespace YapartMarket.Core.Data.Interfaces.Azure
{
    public interface IAzureAliExpressProductRepository: IAzureQueriesGenericRepository<AliExpressProduct>, IAzureCommandGenericRepository<AliExpressProduct>
    {
    }
}
