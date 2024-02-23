using System.Collections.Generic;
using System.Threading.Tasks;
using YapartMarket.Core.Models.Azure;

namespace YapartMarket.Core.Data.Interfaces.Azure
{
    public interface IAliExpressOrderDetailRepository : IAzureQueriesGenericRepository<AliExpressOrderDetail>, IAzureCommandGenericRepository<AliExpressOrderDetail>
    {
        Task UpdateAsync(IEnumerable<AliExpressOrderDetail> orderDetails);
        Task AddAsync(IEnumerable<AliExpressOrderDetail> orderDetails);
    }
}