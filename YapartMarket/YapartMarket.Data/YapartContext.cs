using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using YapartMarket.Core.Models;

namespace YapartMarket.Data
{
   public class YapartContext : DbContext
    {
        public YapartContext(DbContextOptions<YapartContext> options) : base(options)
        {}
        public DbSet<Brand> Brands { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<Section> Sections { get; set; }
        public DbSet<Picture> Pictures { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<CartLine> CartLines { get; set; }
        public DbSet<Mark> Marks { get; set; }
        public DbSet<Model> Models { get; set; }
        public DbSet<Modification> Modifications { get; set; }
        public DbSet<ProductModification> ProductModifications { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new CategoryConfiguration());
            modelBuilder.ApplyConfiguration(new GroupConfiguration());
            modelBuilder.ApplyConfiguration(new ProductConfiguration());
            modelBuilder.ApplyConfiguration(new SectionConfiguration());
            modelBuilder.ApplyConfiguration(new BrandConfiguration());
            modelBuilder.ApplyConfiguration(new PictureConfiguration());

            //modelBuilder.ApplyConfiguration(new ClaimConfiguration());
            //modelBuilder.ApplyConfiguration(new LoginConfiguration());
            //modelBuilder.ApplyConfiguration(new RoleConfiguration());
            //modelBuilder.ApplyConfiguration(new UserConfiguration());

            modelBuilder.ApplyConfiguration(new OrderConfiguration());
            modelBuilder.ApplyConfiguration(new OrderItemConfiguration());
            modelBuilder.ApplyConfiguration(new CartConfiguration());
            modelBuilder.ApplyConfiguration(new CartLineConfiguration());

            modelBuilder.ApplyConfiguration(new MarkConfiguration());
            modelBuilder.ApplyConfiguration(new ModelConfiguration());
            modelBuilder.ApplyConfiguration(new ModificationConfiguration());
            modelBuilder.ApplyConfiguration(new ProductModificationConfiguration());

            //Todo название таблиц SnakeCase
            modelBuilder.NamesToSnakeCase();
        }

        //TODO Нужно для того, чтобы работала инициализация первой миграции
        public class YapartContextDbFactory : IDesignTimeDbContextFactory<YapartContext>
        {
            YapartContext IDesignTimeDbContextFactory<YapartContext>.CreateDbContext(string[] args)
            {
                var optionsBuilder = new DbContextOptionsBuilder<YapartContext>();
                optionsBuilder.UseNpgsql<YapartContext>("Server=localhost;Port=5432;Database=yapart_store;Username=postgres;Password=jfgSvSD@gM;");

                return new YapartContext(optionsBuilder.Options);
            }
        }

    }
}
