using Microsoft.EntityFrameworkCore;
using YapartMarket.Core.Data.Interfaces;
using YapartMarket.Core.Models;

namespace YapartMarket.Data.Implementation
{
    public class ProductRepository : RepositoryBase<Product, int>, IProductRepository
    {
        public ProductRepository(DbContext dbContext) : base(dbContext)
        {}

        protected override object[] GetEntityKeyValues(int id)
        {
            return new object[] { id };
        }

        protected override Product CreateEntityWithId(int id)
        {
            return new Product { Id = id };
        }

        protected override bool CompareEntityId(Product entity, int id)
        {
            return (entity.Id == id);
        }
    }
}
