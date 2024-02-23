using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using YapartMarket.Core.Models.Azure;

namespace YapartMarket.Core.Data.Interfaces.Azure
{
    public interface IAzureProductRepository : IAzureQueriesGenericRepository<Product>, IAzureCommandGenericRepository<Product>
    {
        Task BulkUpdateCountDataAsync(List<Product> list, CancellationToken cancellationToken);
        Task BulkUpdateCountExpressDataAsync(List<Product> list);
        Task BulkUpdateTakeTimeAsync(List<Product> list);
        Task BulkUpdateExpressTakeTimeAsync(List<Product> list);
        Task BulkUpdateProductIdAsync(IReadOnlyList<Product> products);
    }
}
