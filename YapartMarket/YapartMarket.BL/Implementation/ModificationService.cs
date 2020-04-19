using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using YapartMarket.Core.BL;
using YapartMarket.Core.Data;
using YapartMarket.Core.Data.Interfaces;
using YapartMarket.Core.Models;

namespace YapartMarket.BL.Implementation
{
    public class ModificationService : RepositoryAwareServiceBase, IModificationService
    {
        public ModificationService(IRepositoryFactory repositoryFactory) : base(repositoryFactory)
        {
        }

        public Modification Add(Modification modification)
        {
            var modificationRepository = RepositoryFactory.GetRepository<IModificationRepository>();
            modificationRepository.Add(modification);
            return modification;
        }

        public Modification Update(Modification modification)
        {
            var modificationRepository = RepositoryFactory.GetRepository<IModificationRepository>();
            modificationRepository.Update(modification);
            return modification;
        }

        public Modification GetById(int id)
        {
            var modificationRepositoey = RepositoryFactory.GetRepository<IModificationRepository>();
            var model = modificationRepositoey.GetById(id);
            return model;
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
