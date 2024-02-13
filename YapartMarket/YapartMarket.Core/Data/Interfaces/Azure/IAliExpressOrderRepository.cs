using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using YapartMarket.Core.Models.Azure;

namespace YapartMarket.Core.Data.Interfaces.Azure
{
    public interface IAliExpressOrderRepository : IAzureQueriesGenericRepository<AliExpressOrder>, IAzureCommandGenericRepository<AliExpressOrder>
    {
        Task AddOrdersAsync(IEnumerable<AliExpressOrder> aliExpressOrders);
        Task UpdateAsync(IEnumerable<AliExpressOrder> aliExpressOrders);
        Task<IEnumerable<AliExpressOrder>> GetOrdersByWaitSellerSendGoodsAsync(DateTime start, DateTime end);
    }
}
