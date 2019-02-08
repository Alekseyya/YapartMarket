using System;
using System.Collections.Generic;
using System.Text;
using YapartMarket.Core.BL;
using YapartMarket.Core.Data;

namespace YapartMarket.BL.Implementation
{
   public class OrderItemService : RepositoryAwareServiceBase, IOrderItemService
    {
        public OrderItemService(IRepositoryFactory repositoryFactory) : base(repositoryFactory)
        {
        }
    }
}
