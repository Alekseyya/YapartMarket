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
   public class MarkService : RepositoryAwareServiceBase, IMarkService
    {
        public MarkService(IRepositoryFactory repositoryFactory) : base(repositoryFactory)
        {
        }

        public Mark Add(Mark mark)
        {
            var markRepository = RepositoryFactory.GetRepository<IMarkRepository>();
            markRepository.Add(mark);
            return mark;
        }

        public Mark Update(Mark mark)
        {
            var markRepository = RepositoryFactory.GetRepository<IMarkRepository>();
            markRepository.Update(mark);
            return mark;
        }

        public Mark GetById(int id)
        {
            var markRepository = RepositoryFactory.GetRepository<IMarkRepository>();
            var mark = markRepository.GetById(id);
            return mark;
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
    }
}
