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
   public class ModelService : GenericService<Model, int, IModelRepository>, IModelService
    {
        public ModelService(IRepositoryFactory repositoryFactory) : base(repositoryFactory)
        {

        }
        public IList<Model> GetAll()
        {
            return GetAll(null);
        }

        public IList<Model> GetAll(Expression<Func<Model, bool>> conditionFunc)
        {
            var modelRepository = RepositoryFactory.GetRepository<IModelRepository>();
            IList<Model> models;
            if (conditionFunc != null)
            {
                models = modelRepository.GetAll().AsQueryable().Where(conditionFunc).ToList();
            }
            return modelRepository.GetAll();
        }
    }
}
