using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace YapartMarket.Core.Data
{
    public abstract class SingletonDbAccessorBase<TDbContext> : IDbAccessor<TDbContext>, IDisposable where TDbContext : DbContext
    {
        private bool _disposed = false;

        protected TDbContext DbContext { get; set; } = null;

        public TDbContext GetDbContext()
        {
            if (_disposed)
                throw new ObjectDisposedException("DbContext");

            if (DbContext == null)
                DbContext = CreateDbContext();
            return DbContext;
        }

        public void Dispose()
        {
            if (_disposed)
                return;
            Dispose(true);
            _disposed = true;
            GC.SuppressFinalize(this);
        }

        protected abstract TDbContext CreateDbContext();

        protected virtual void Dispose(bool disposing)
        {
            if (DbContext != null)
            {
                DbContext.Dispose();
                DbContext = null;
            }
        }
    }
}
