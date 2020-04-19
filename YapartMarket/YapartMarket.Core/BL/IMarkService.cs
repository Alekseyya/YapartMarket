using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using YapartMarket.Core.Models;

namespace YapartMarket.Core.BL
{
    public interface IMarkService
    {
        Mark Add(Mark mark);
        Mark Update(Mark mark);
        Mark GetById(int id);
        IList<Mark> GetAll();
        IList<Mark> GetAll(Expression<Func<Mark, bool>> conditionFunc);
        IList<Mark> GetAll(Expression<Func<Mark, bool>> conditionFunc, Func<IQueryable<Mark>, IOrderedQueryable<Mark>> sortFunc);
        IList<Mark> GetAll(Expression<Func<Mark, bool>> conditionFunc, Func<IQueryable<Mark>, IOrderedQueryable<Mark>> sortFunc, Func<IQueryable<Mark>, IQueryable<Mark>> includeFuncs);
    }
}
