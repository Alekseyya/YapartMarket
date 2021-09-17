using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using YapartMarket.Core.BL.Queries;
using YapartMarket.Core.DateStructures;
using YapartMarket.Core.DTO;
using YapartMarket.Core.Models.Azure;

namespace YapartMarket.Core.BL
{
    public interface IAliExpressOrderService : IAliExpressOrderQueries
    {
        List<AliExpressOrderDTO> QueryOrderDetail(DateTime? startDateTime = null, DateTime? endDateTime = null, List<OrderStatus> orderStatusList = null);
        Task AddOrders(List<AliExpressOrder> aliExpressOrders);
        Task<IEnumerable<AliExpressOrder>> GetOrders(DateTime start, DateTime end);
    }
}
