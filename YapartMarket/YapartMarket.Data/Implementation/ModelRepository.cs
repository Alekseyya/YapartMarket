using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using YapartMarket.Core.Data.Interfaces;
using YapartMarket.Core.Models;

namespace YapartMarket.Data.Implementation
{
   public class ModelRepository : RepositoryBase<Model, int>, IModelRepository
    {
        public ModelRepository(DbContext dbContext) : base(dbContext)
        {}

        protected override object[] GetEntityKeyValues(int id)
        {
            return new object[] { id };
        }

        protected override Model CreateEntityWithId(int id)
        {
            return new Model { Id = id };
        }

        protected override bool CompareEntityId(Model entity, int id)
        {
            return (entity.Id == id);
        }
    }
}
