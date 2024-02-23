using System.Collections.Generic;
using System.Threading.Tasks;
using YapartMarket.Core.Models.Azure;

namespace YapartMarket.Core.Data.Interfaces.Azure
{
    public interface IAzureAliExpressProductRepository: IAzureQueriesGenericRepository<AliExpressProduct>, IAzureCommandGenericRepository<AliExpressProduct>
    {
        Task BulkUpdateDataAsync(IReadOnlyList<AliExpressProduct> list);
    }
}
