using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace YapartMarket.Core.Data
{
   public interface IGenericRepository<TEntity, TId>
    {
        TEntity GetById(TId id);

        IList<TEntity> GetAll();

        IList<TEntity> GetAll(Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> sortFunc);

        IList<TEntity> GetAll(Func<IQueryable<TEntity>, IQueryable<TEntity>> includeFuncs);

        IList<TEntity> GetAll(Expression<Func<TEntity, bool>> condition);

        IList<TEntity> GetAll(Expression<Func<TEntity, bool>> condition, Func<IQueryable<TEntity>,
            IOrderedQueryable<TEntity>> sortFunc, Func<IQueryable<TEntity>, IQueryable<TEntity>> includeFuncs);

        int GetCount();

        int GetCount(Expression<Func<TEntity, bool>> condition);

        IList<TEntity> GetWindow(int startFrom, int windowSize,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> sortFunc);

        IList<TEntity> GetWindow(Expression<Func<TEntity, bool>> condition, int startFrom, int windowSize,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> sortFunc);

        Task<TEntity> AddAsync(TEntity entry);
        TEntity Add(TEntity entity);

        TEntity Update(TEntity entity);

        void RemoveRange(IList<TEntity> entries);
        Task RemoveRangeAsync(IList<TEntity> entries);
        void Delete(TEntity entity);

        void Delete(TId id);

        IQueryable<TEntity> GetQueryable();
    }
}
