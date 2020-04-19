using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace YapartMarket.Core.Data
{
   public interface IRepository<TEntity, TId>
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

        TEntity Add(TEntity entity);

        TEntity Update(TEntity entity);

        void Delete(TEntity entity);

        void Delete(TId id);

        IQueryable<TEntity> GetQueryable();
    }
}
