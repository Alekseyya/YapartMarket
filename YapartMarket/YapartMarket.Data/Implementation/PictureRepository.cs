using Microsoft.EntityFrameworkCore;
using YapartMarket.Core.Data.Interfaces;
using YapartMarket.Core.Models;

namespace YapartMarket.Data.Implementation
{
    public class PictureRepository : RepositoryBase<Picture, int>, IPictureRepository
    {
        public PictureRepository(DbContext dbContext) : base(dbContext)
        {}

        protected override object[] GetEntityKeyValues(int id)
        {
            return new object[] { id };
        }

        protected override Picture CreateEntityWithId(int id)
        {
            return new Picture { Id = id };
        }

        protected override bool CompareEntityId(Picture entity, int id)
        {
            return (entity.Id == id);
        }
    }
}
