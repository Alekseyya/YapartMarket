using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using YapartMarket.Core.Models.Azure;

namespace YapartMarket.Core.Data.Interfaces.Azure
{
    public interface IAzureProductRepository : IAzureQueriesGenericRepository<Product>, IAzureCommandGenericRepository<Product>
    {
        Task BulkUpdateCountDataAsync(List<Product> list, CancellationToken cancellationToken);
        Task BulkUpdateCountExpressData(List<Product> list);
        Task BulkUpdateTakeTime(List<Product> list);
        Task BulkUpdateExpressTakeTime(List<Product> list);
        Task BulkUpdateProductId(IReadOnlyList<Product> products);
    }
}
