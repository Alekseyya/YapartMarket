using System.Collections.Generic;
using System.Threading.Tasks;
using YapartMarket.Core.Models.Azure;

namespace YapartMarket.Core.Data.Interfaces.Azure
{
    public interface IAliExpressOrderDetailRepository : IAzureQueriesGenericRepository<AliExpressOrderDetail>, IAzureCommandGenericRepository<AliExpressOrderDetail>
    {
        Task Update(IEnumerable<AliExpressOrderDetail> orderDetails);
        Task Add(IEnumerable<AliExpressOrderDetail> orderDetails);
    }
}