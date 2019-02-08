using Microsoft.EntityFrameworkCore;
using YapartMarket.Core.Data.Interfaces;
using YapartMarket.Core.Models;

namespace YapartMarket.Data.Implementation
{
   public class CartLineRepository: RepositoryBase<CartLine, int>, ICartLineRepository
    {
        public CartLineRepository(DbContext dbContext) : base(dbContext)
        {}

        protected override object[] GetEntityKeyValues(int id)
        {
            return new object[] { id };
        }

        protected override CartLine CreateEntityWithId(int id)
        {
            return new CartLine { Id = id };
        }

        protected override bool CompareEntityId(CartLine entity, int id)
        {
            return (entity.Id == id);
        }
    }
}
