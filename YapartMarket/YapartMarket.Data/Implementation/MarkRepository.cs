using Microsoft.EntityFrameworkCore;
using YapartMarket.Core.Data.Interfaces;
using YapartMarket.Core.Models;

namespace YapartMarket.Data.Implementation
{
   public class MarkRepository : RepositoryBase<Mark, int>, IMarkRepository
    {
        public MarkRepository(DbContext dbContext) : base(dbContext)
        {}

        protected override object[] GetEntityKeyValues(int id)
        {
            return new object[] { id };
        }

        protected override Mark CreateEntityWithId(int id)
        {
            return new Mark { Id = id };
        }

        protected override bool CompareEntityId(Mark entity, int id)
        {
            return (entity.Id == id);
        }
    }
}
