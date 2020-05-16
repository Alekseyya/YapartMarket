using Microsoft.EntityFrameworkCore;
using YapartMarket.Core.Data.Interfaces;
using YapartMarket.Core.Models;

namespace YapartMarket.Data.Implementation
{
   public class ModificationRepository : GenericRepository<Modification, int>, IModificationRepository
    {
        public ModificationRepository(DbContext dbContext) : base(dbContext)
        {}

        protected override object[] GetEntityKeyValues(int id)
        {
            return new object[] { id };
        }

        protected override Modification CreateEntityWithId(int id)
        {
            return new Modification { Id = id };
        }

        protected override bool CompareEntityId(Modification entity, int id)
        {
            return (entity.Id == id);
        }
    }
}
