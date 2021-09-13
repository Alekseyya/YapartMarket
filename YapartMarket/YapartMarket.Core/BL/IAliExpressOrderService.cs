﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using YapartMarket.Core.BL.Queries;
using YapartMarket.Core.DateStructures;
using YapartMarket.Core.DTO;

namespace YapartMarket.Core.BL
{
    public interface IAliExpressOrderService : IAliExpressOrderQueries
    {
        List<AliExpressOrderListDTO> QueryOrderDetail(DateTime? startDateTime = null, DateTime? endDateTime = null, List<OrderStatus> orderStatusList = null);
        Task AddOrders(List<AliExpressOrderListDTO> aliExpressOrderListDtos);
    }
}
