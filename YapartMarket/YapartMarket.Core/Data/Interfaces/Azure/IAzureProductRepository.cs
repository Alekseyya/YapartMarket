using System.Collections.Generic;
using System.Threading.Tasks;
using YapartMarket.Core.Models.Azure;

namespace YapartMarket.Core.Data.Interfaces.Azure
{
    public interface IAzureProductRepository : IAzureQueriesGenericRepository<Product>, IAzureCommandGenericRepository<Product>
    {
        Task BulkUpdateCountData(List<Product> list);
    }
}
