using Microsoft.EntityFrameworkCore;
using YapartMarket.Core.Data.Interfaces;
using YapartMarket.Core.Models;

namespace YapartMarket.Data.Implementation
{
    public class SectionRepository : GenericRepository<Section, int>, ISectionRepository
    {
        public SectionRepository(DbContext dbContext) : base(dbContext)
        {}

        protected override object[] GetEntityKeyValues(int id)
        {
            return new object[] { id };
        }

        protected override Section CreateEntityWithId(int id)
        {
            return new Section { Id = id };
        }

        protected override bool CompareEntityId(Section entity, int id)
        {
            return (entity.Id == id);
        }
    }
}
