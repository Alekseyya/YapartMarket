
using Microsoft.EntityFrameworkCore;
using YapartMarket.Core.Data.Interfaces;
using YapartMarket.Core.Models;

namespace YapartMarket.Data.Implementation
{
    public class BrandRepository : RepositoryBase<Brand, int>, IBrandRepository
    {
        public BrandRepository(DbContext dbContext) :base(dbContext)
        {}

        protected override object[] GetEntityKeyValues(int id)
        {
            return new object[] { id };
        }

        protected override Brand CreateEntityWithId(int id)
        {
            return new Brand { Id = id };
        }

        protected override bool CompareEntityId(Brand entity, int id)
        {
            return (entity.Id == id);
        }
    }
}
