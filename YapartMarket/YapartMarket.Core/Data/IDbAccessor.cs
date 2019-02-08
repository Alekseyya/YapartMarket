using Microsoft.EntityFrameworkCore;

namespace YapartMarket.Core.Data
{
   public interface IDbAccessor<TDbContext> where TDbContext : DbContext
    {
        TDbContext GetDbContext();
    }
}
