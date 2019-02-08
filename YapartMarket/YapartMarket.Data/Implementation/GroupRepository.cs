using Microsoft.EntityFrameworkCore;
using YapartMarket.Core.Data.Interfaces;
using YapartMarket.Core.Models;

namespace YapartMarket.Data.Implementation
{
    public class GroupRepository : RepositoryBase<Group, int>, IGroupRepository
    {
        public GroupRepository(DbContext dbContext) : base(dbContext)
        {
        }

        protected override object[] GetEntityKeyValues(int id)
        {
            return new object[] { id };
        }

        protected override Group CreateEntityWithId(int id)
        {
            return new Group { Id = id };
        }

        protected override bool CompareEntityId(Group entity, int id)
        {
            return (entity.Id == id);
        }
    }
}
