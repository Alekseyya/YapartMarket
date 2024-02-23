using Microsoft.EntityFrameworkCore;
using YapartMarket.Core.Models;

namespace YapartMarket.Data
{
   public class YapartContext : DbContext
    {
        public YapartContext(DbContextOptions<YapartContext> options) : base(options)
        {}
        
        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
        }
    }
}
