using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using YapartMarket.Core.BL;
using YapartMarket.Core.Data;
using YapartMarket.Core.Data.Interfaces;
using YapartMarket.Core.Models;

namespace YapartMarket.BL.Implementation
{
    public class ModificationService : GenericService<Modification,int, IModificationRepository>, IModificationService
    {
        public ModificationService(IRepositoryFactory repositoryFactory) : base(repositoryFactory)
        {
        }

        public IList<Modification> GetAll()
        {
            return GetAll(null);
        }

        public IList<Modification> GetAll(Expression<Func<Modification, bool>> conditionFunc)
        {
            var modificationRepositoey = RepositoryFactory.GetRepository<IModificationRepository>();
            IList<Modification> models;
            if (conditionFunc != null)
            {
                models = modificationRepositoey.GetAll().AsQueryable().Where(conditionFunc).ToList();
            }
            return modificationRepositoey.GetAll();
        }
    }
}
