using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using YapartMarket.Core.BL;
using YapartMarket.Core.Data;
using YapartMarket.Core.Data.Interfaces;
using YapartMarket.Core.Models;

namespace YapartMarket.BL.Implementation
{
    public class MarkService : GenericService<Mark, int, IMarkRepository>, IMarkService
    {
        public MarkService(IRepositoryFactory repositoryFactory) : base(repositoryFactory)
        {
        }
        public IList<Mark> GetAll()
        {
            return GetAll(null);
        }

        public IList<Mark> GetAll(Expression<Func<Mark, bool>> conditionFunc)
        {
            return GetAll(conditionFunc, null, null);
        }

        public IList<Mark> GetAll(Expression<Func<Mark, bool>> conditionFunc, Func<IQueryable<Mark>, IOrderedQueryable<Mark>> sortFunc)
        {
            return GetAll(conditionFunc, sortFunc, null);
        }

        public IList<Mark> GetAll(Expression<Func<Mark, bool>> conditionFunc, Func<IQueryable<Mark>, IOrderedQueryable<Mark>> sortFunc, Func<IQueryable<Mark>, IQueryable<Mark>> includeFuncs)
        {
            var markRepository = RepositoryFactory.GetRepository<IMarkRepository>();
            IList<Mark> marks = null;
            if (conditionFunc != null && sortFunc == null && includeFuncs == null)
            {
                marks = markRepository.GetAll(conditionFunc, null, null).AsQueryable().ToList();

            }else if (conditionFunc != null && sortFunc != null && includeFuncs == null)
            {
                marks = markRepository.GetAll(conditionFunc, sortFunc, null).AsQueryable().ToList();
            }
            else if(conditionFunc != null && sortFunc == null && includeFuncs != null)
            {
                marks = markRepository.GetAll(conditionFunc, null, includeFuncs).AsQueryable().ToList();
            }
            else if(conditionFunc != null && sortFunc != null && includeFuncs != null)
            {
                marks = markRepository.GetAll(conditionFunc, sortFunc,includeFuncs).AsQueryable().ToList();
            }
            else if(conditionFunc == null && sortFunc == null && includeFuncs == null)
            {
                marks = markRepository.GetAll().AsQueryable().ToList();
            }
            return marks;
        }

        public Task<IList<Mark>> GetAllAsync()
        {
            return GetAllAsync(null);
        }

        public Task<IList<Mark>> GetAllAsync(Expression<Func<Mark, bool>> conditionFunc)
        {
            return GetAllAsync(conditionFunc, null, null);
        }

        public Task<IList<Mark>> GetAllAsync(Expression<Func<Mark, bool>> conditionFunc, Func<IQueryable<Mark>, IOrderedQueryable<Mark>> sortFunc)
        {
            return GetAllAsync(conditionFunc, sortFunc, null);
        }

        public async Task<IList<Mark>> GetAllAsync(Expression<Func<Mark, bool>> conditionFunc, Func<IQueryable<Mark>, IOrderedQueryable<Mark>> sortFunc, Func<IQueryable<Mark>, IQueryable<Mark>> includeFuncs)
        {
            var markRepository = RepositoryFactory.GetRepository<IMarkRepository>();
            IList<Mark> marks = null;
            if (conditionFunc != null && sortFunc == null && includeFuncs == null)
            {
                marks = await markRepository.GetAll(conditionFunc, null, null).AsQueryable().ToListAsync();

            }
            else if (conditionFunc != null && sortFunc != null && includeFuncs == null)
            {
                marks = await markRepository.GetAll(conditionFunc, sortFunc, null).AsQueryable().ToListAsync();
            }
            else if (conditionFunc != null && sortFunc == null && includeFuncs != null)
            {
                marks = await markRepository.GetAll(conditionFunc, null, includeFuncs).AsQueryable().ToListAsync();
            }
            else if (conditionFunc != null && sortFunc != null && includeFuncs != null)
            {
                marks = await markRepository.GetAll(conditionFunc, sortFunc, includeFuncs).AsQueryable().ToListAsync();
            }
            else if (conditionFunc == null && sortFunc == null && includeFuncs == null)
            {
                marks = await markRepository.GetAll().AsQueryable().ToListAsync();
            }
            return marks;
        }
    }
}
