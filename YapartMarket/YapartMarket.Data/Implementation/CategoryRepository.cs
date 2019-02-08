using Microsoft.EntityFrameworkCore;
using YapartMarket.Core.Data.Interfaces;
using YapartMarket.Core.Models;

namespace YapartMarket.Data.Implementation
{
   public class CategoryRepository : RepositoryBase<Category, int>, ICategoryRepository
    {
        public CategoryRepository(DbContext dbContext) : base(dbContext)
        {
        }

        protected override object[] GetEntityKeyValues(int id)
        {
            return new object[] { id };
        }

        protected override Category CreateEntityWithId(int id)
        {
            return new Category { Id = id };
        }

        protected override bool CompareEntityId(Category entity, int id)
        {
            return (entity.Id == id);
        }
    }
}
