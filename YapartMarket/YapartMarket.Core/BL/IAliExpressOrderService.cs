using System;
using System.Collections.Generic;
using YapartMarket.Core.BL.Queries;
using YapartMarket.Core.DTO;

namespace YapartMarket.Core.BL
{
    public interface IAliExpressOrderService : IAliExpressOrderQueries
    {
        List<AliExpressOrderListDTO> QueryOrderDetail(DateTime? startDateTime = null, DateTime? endDateTime = null);
    }
}
