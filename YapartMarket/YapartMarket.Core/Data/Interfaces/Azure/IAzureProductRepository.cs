using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using YapartMarket.Core.Models.Azure;

namespace YapartMarket.Core.Data.Interfaces.Azure
{
    public interface IAzureProductRepository : IAzureQueriesGenericRepository<Product>, IAzureCommandGenericRepository<Product>
    {
        Task<string> BulkUpdateCountDataAsync(List<Product> list, CancellationToken cancellationToken);
        Task<string> BulkUpdateCountExpressDataAsync(List<Product> list, CancellationToken cancellationToken);
        Task BulkUpdateTakeTimeAsync(List<Product> list, CancellationToken cancellationToken);
        Task BulkUpdateExpressTakeTimeAsync(List<Product> list, CancellationToken cancellationToken);
        Task BulkUpdateProductIdAsync(IReadOnlyList<Product> products, CancellationToken cancellationToken);
    }
}
