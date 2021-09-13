using System.Collections.Generic;
using System.Threading.Tasks;
using YapartMarket.Core.DTO;
using YapartMarket.Core.Models.Azure;

namespace YapartMarket.Core.Data.Interfaces.Azure
{
    public interface IAzureAliExpressOrderRepository : IAzureQueriesGenericRepository<AliExpressOrder>, IAzureCommandGenericRepository<AliExpressOrder>
    {
        Task AddOrders(IEnumerable<AliExpressOrderListDTO> aliExpressOrderListDtos);
    }
}
