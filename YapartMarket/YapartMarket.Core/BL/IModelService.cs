using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using YapartMarket.Core.Models;

namespace YapartMarket.Core.BL
{
    public interface IModelService
    {
        Model Add(Model model);
        Model Update(Model model);
        Model GetById(int id);
        IList<Model> GetAll();
        IList<Model> GetAll(Expression<Func<Model, bool>> conditionFunc);
    }
}
