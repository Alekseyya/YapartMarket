using Microsoft.EntityFrameworkCore;
using YapartMarket.Core.Data.Interfaces;
using YapartMarket.Core.Models;

namespace YapartMarket.Data.Implementation
{
   public class OrderItemRepository : RepositoryBase<OrderItem, int>, IOrderItemRepository
    {
        public OrderItemRepository(DbContext dbContext) : base(dbContext)
        {}

        protected override object[] GetEntityKeyValues(int id)
        {
            return new object[] { id };
        }

        protected override OrderItem CreateEntityWithId(int id)
        {
            return new OrderItem { Id = id };
        }

        protected override bool CompareEntityId(OrderItem entity, int id)
        {
            return (entity.Id == id);
        }
    }
}
