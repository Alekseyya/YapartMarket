using Microsoft.EntityFrameworkCore;
using YapartMarket.Core.Data.Interfaces;
using YapartMarket.Core.Models;

namespace YapartMarket.Data.Implementation
{
   public class OrderRepository : RepositoryBase<Order, int>, IOrderItemRepository
    {
        public OrderRepository(DbContext dbContext) : base(dbContext)
        {}

        protected override object[] GetEntityKeyValues(int id)
        {
            return new object[] { id };
        }

        protected override Order CreateEntityWithId(int id)
        {
            return new Order { Id = id };
        }

        protected override bool CompareEntityId(Order entity, int id)
        {
            return (entity.Id == id);
        }
    }
}
