﻿using System;
using System.Collections.Generic;
using System.Text;
using YapartMarket.Core.Models;

namespace YapartMarket.Core.Data.Interfaces
{
    public interface IOrderItemRepository : IGenericRepository<OrderItem, int>
    {
    }
}
