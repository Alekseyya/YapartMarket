using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using YapartMarket.Core.Data;

namespace YapartMarket.Data.Implementation
{
    public sealed class YapartSingletonDbAccessor : SingletonDbAccessorBase<YapartContext>, IYapartDbAccessor
    {
        IDbContextTransaction _transaction = null!;

        private readonly DbContextOptions<YapartContext> _options;

        public YapartSingletonDbAccessor(DbContextOptions<YapartContext> options)
        {
            if (options == null)
                throw new ArgumentNullException(nameof(options));

            _options = options;
        }

        protected override YapartContext CreateDbContext()
        {
            throw new NotImplementedException();
        }

        public new DbContext GetDbContext()
        {
            return new YapartContext(_options);
        }

        public void EnsureTransaction()
        {
            if (_transaction == null)
            {
                var dbContext = GetDbContext();
                var database = dbContext.Database;
                _transaction = database.BeginTransaction();
            }
        }

        public void CommitTransaction()
        {
            if (_transaction != null)
            {
                _transaction.Commit();
                _transaction.Dispose();
                _transaction = null!;
            }
        }

        public void RollbackTransaction()
        {
            if (_transaction != null)
            {
                _transaction.Rollback();
                _transaction.Dispose();
                _transaction = null!;
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                RollbackTransaction();
            }
            base.Dispose(disposing);
        }
    }
}
