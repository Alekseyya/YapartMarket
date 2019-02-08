using Microsoft.EntityFrameworkCore;
using YapartMarket.Core.Data.Interfaces;
using YapartMarket.Core.Models;

namespace YapartMarket.Data.Implementation
{
   public class CartRepository: RepositoryBase<Cart, int>, ICartRepository
    {
        public CartRepository(DbContext dbContext) : base(dbContext)
        {
        }

        protected override object[] GetEntityKeyValues(int id)
        {
            return new object[] { id };
        }

        protected override Cart CreateEntityWithId(int id)
        {
            return new Cart { Id = id };
        }

        protected override bool CompareEntityId(Cart entity, int id)
        {
            return (entity.Id == id);
        }
    }
}
