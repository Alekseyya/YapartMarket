using Microsoft.EntityFrameworkCore;
using YapartMarket.Core.Data.Interfaces;
using YapartMarket.Core.Models;

namespace YapartMarket.Data.Implementation
{
    public class ProductModificationRepository : RepositoryBase<ProductModification, int>, IProductModificationRepository
    {
        public ProductModificationRepository(DbContext dbContext) : base(dbContext)
        {}

        protected override object[] GetEntityKeyValues(int id)
        {
            return new object[] { id };
        }

        protected override ProductModification CreateEntityWithId(int id)
        {
            return new ProductModification { Id = id };
        }

        protected override bool CompareEntityId(ProductModification entity, int id)
        {
            return (entity.Id == id);
        }
    }
}
