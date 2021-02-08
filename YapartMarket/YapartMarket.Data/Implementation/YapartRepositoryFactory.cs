using System;
using YapartMarket.Core.Data;
using YapartMarket.Core.Data.Interfaces;
using YapartMarket.Core.Data.Interfaces.Access;
using YapartMarket.Data.Implementation.Access;

namespace YapartMarket.Data.Implementation
{
    public class YapartRepositoryFactory : IRepositoryFactory
    {
        protected  IYapartDbAccessor DbAccessor { get; }
        public YapartRepositoryFactory(IYapartDbAccessor dbAccessor)
        {
            if (dbAccessor == null)
                throw new ArgumentNullException(nameof(dbAccessor));

            DbAccessor = dbAccessor;
        }
        public IRepository GetRepository<IRepository>()
        {
            var repositoryType = typeof(IRepository);
            object result;
            if (repositoryType == typeof(IBrandRepository))
                result = new BrandRepository(DbAccessor.GetDbContext());
            else if(repositoryType == typeof(ICartLineRepository))
                result = new CartLineRepository(DbAccessor.GetDbContext());
            else if (repositoryType == typeof(ICartRepository))
                result = new CartRepository(DbAccessor.GetDbContext());
            else if (repositoryType == typeof(ICategoryRepository))
                result = new CategoryRepository(DbAccessor.GetDbContext());
            else if (repositoryType == typeof(IGroupRepository))
                result = new GroupRepository(DbAccessor.GetDbContext());
            else if (repositoryType == typeof(IMarkRepository))
                result = new MarkRepository(DbAccessor.GetDbContext());
            else if (repositoryType == typeof(IModelRepository))
                result = new ModelRepository(DbAccessor.GetDbContext());
            else if (repositoryType == typeof(IModificationRepository))
                result = new ModificationRepository(DbAccessor.GetDbContext());
            else if (repositoryType == typeof(IOrderItemRepository))
                result = new OrderItemRepository(DbAccessor.GetDbContext());
            else if (repositoryType == typeof(IOrderRepository))
                result = new OrderRepository(DbAccessor.GetDbContext());
            else if (repositoryType == typeof(IPictureRepository))
                result = new PictureRepository(DbAccessor.GetDbContext());
            else if (repositoryType == typeof(IProductModificationRepository))
                result = new ProductModificationRepository(DbAccessor.GetDbContext());
            else if (repositoryType == typeof(IProductRepository))
                result = new ProductRepository(DbAccessor.GetDbContext());
            else if (repositoryType == typeof(ISectionRepository))
                result = new SectionRepository(DbAccessor.GetDbContext());
            else
                throw new RepositoryNotFoundException(repositoryType);
            return (IRepository)result;
        }
    }
}
