using System;
using System.Collections.Generic;
using System.Text;
using YapartMarket.Core.Data;

namespace YapartMarket.Data.Implementation
{
   public class YapartUnitOfWork : IUnitOfWork
    {
        private readonly IYapartDbAccessor _dbAccessor;

        public YapartUnitOfWork(IYapartDbAccessor dbAccessor)
        {
            _dbAccessor = dbAccessor;
        }
        public void EnsureTransaction()
        {
            _dbAccessor.EnsureTransaction();
        }

        public void CommitTransaction()
        {
            _dbAccessor.CommitTransaction();
        }

        public void RollbackTransaction()
        {
            _dbAccessor.RollbackTransaction();
        }
    }
}
