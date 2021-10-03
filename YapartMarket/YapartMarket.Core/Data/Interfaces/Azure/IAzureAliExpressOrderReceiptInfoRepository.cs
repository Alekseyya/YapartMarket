using System.Collections.Generic;
using System.Threading.Tasks;
using YapartMarket.Core.Models.Azure;

namespace YapartMarket.Core.Data.Interfaces.Azure
{
    public interface IAzureAliExpressOrderReceiptInfoRepository : IAzureQueriesGenericRepository<AliExpressOrderReceiptInfo>, IAzureCommandGenericRepository<AliExpressOrderReceiptInfo>
    {
        Task InsertAsync(IEnumerable<AliExpressOrderReceiptInfo> collection);
        Task InsertAsync(AliExpressOrderReceiptInfo aliExpressOrderReceiptInfo);
    }
}
