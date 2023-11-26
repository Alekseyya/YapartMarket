using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using YapartMarket.Core.Models;

namespace YapartMarket.Core.BL
{
    public interface IMarkService
    {
        IList<Mark> GetAll();
        IList<Mark> GetAll(Expression<Func<Mark, bool>> conditionFunc);
        IList<Mark> GetAll(Expression<Func<Mark, bool>> conditionFunc, Func<IQueryable<Mark>, IOrderedQueryable<Mark>> sortFunc);
        IList<Mark> GetAll(Expression<Func<Mark, bool>> conditionFunc, Func<IQueryable<Mark>, IOrderedQueryable<Mark>> sortFunc, Func<IQueryable<Mark>, IQueryable<Mark>> includeFuncs);

        Task<IList<Mark>> GetAllAsync();
        Task<IList<Mark>> GetAllAsync(Expression<Func<Mark, bool>> conditionFunc);
        Task<IList<Mark>> GetAllAsync(Expression<Func<Mark, bool>> conditionFunc, Func<IQueryable<Mark>, IOrderedQueryable<Mark>> sortFunc);
        Task<IList<Mark>> GetAllAsync(Expression<Func<Mark, bool>> conditionFunc, Func<IQueryable<Mark>, IOrderedQueryable<Mark>> sortFunc, Func<IQueryable<Mark>, IQueryable<Mark>> includeFuncs);
    }
}
