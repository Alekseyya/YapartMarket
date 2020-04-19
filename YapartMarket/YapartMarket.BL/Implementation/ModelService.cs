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
   public class ModelService : RepositoryAwareServiceBase, IModelService
    {
        public ModelService(IRepositoryFactory repositoryFactory) : base(repositoryFactory)
        {

        }

        public Model Add(Model model)
        {
            var modelRepository = RepositoryFactory.GetRepository<IModelRepository>();
            modelRepository.Add(model);
            return model;
        }

        public Model Update(Model model)
        {
            var modelRepository = RepositoryFactory.GetRepository<IModelRepository>();
            modelRepository.Update(model);
            return model;
        }

        public Model GetById(int id)
        {
            var modelRepository = RepositoryFactory.GetRepository<IModelRepository>();
            var model = modelRepository.GetById(id);
            return model;
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
