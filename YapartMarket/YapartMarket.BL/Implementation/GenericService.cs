using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using YapartMarket.Core.Data;

namespace YapartMarket.BL.Implementation
{
   public abstract class GenericService<TEntry, TId, TRepository> where TRepository : IGenericRepository<TEntry, TId>
    {
        protected IRepositoryFactory RepositoryFactory { get; }

        protected GenericService(IRepositoryFactory repositoryFactory)
        {
            if (repositoryFactory == null)
                throw new ArgumentNullException(nameof(repositoryFactory));

            RepositoryFactory = repositoryFactory;
        }
        public virtual TEntry Add(TEntry entry)
        {
            if (entry == null) throw new ArgumentNullException(nameof(entry));
            return RepositoryFactory.GetRepository<TRepository>().Add(entry);
        }

        public virtual Task<TEntry> AddAsync(TEntry entry)
        {
            if (entry == null) throw new ArgumentNullException(nameof(entry));
            return RepositoryFactory.GetRepository<TRepository>().AddAsync(entry);
        }
        public virtual void Delete(TId id)
        {
            RepositoryFactory.GetRepository<TRepository>().Delete(id);
        }

        public virtual void RemoveRange(IList<TEntry> entries)
        {
            RepositoryFactory.GetRepository<TRepository>().RemoveRange(entries);
        }

        public virtual Task RemoveRangeAsync(IList<TEntry> entries)
        {
            RepositoryFactory.GetRepository<TRepository>().RemoveRangeAsync(entries);
            return Task.CompletedTask;
        }

        public virtual TEntry GetById(TId id)
        {
            return RepositoryFactory.GetRepository<TRepository>().GetById(id);
        }

        public virtual TEntry Update(TEntry entry)
        {
            if (entry == null) throw new ArgumentNullException(nameof(entry));
            return RepositoryFactory.GetRepository<TRepository>().Update(entry);
        }

        public virtual List<TEntry> Get(Func<IQueryable<TEntry>, List<TEntry>> delegateExpr)
        {
            if (delegateExpr == null) throw new ArgumentNullException(nameof(delegateExpr));
            var repositoryQuery = RepositoryFactory.GetRepository<TRepository>().GetQueryable();
            return delegateExpr(repositoryQuery);
        }

        public virtual async Task<List<TEntry>> Get()
        {
            return await RepositoryFactory.GetRepository<TRepository>().GetQueryable().ToListAsync();
        }
    }
}
