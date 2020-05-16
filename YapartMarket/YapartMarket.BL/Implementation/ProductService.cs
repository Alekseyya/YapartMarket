using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using YapartMarket.Core.BL;
using YapartMarket.Core.Data;
using YapartMarket.Core.Data.Interfaces;
using YapartMarket.Core.Models;

namespace YapartMarket.BL.Implementation
{
    public class ProductService : GenericService<Product, int, IProductRepository>, IProductService
    {
        public ProductService(IRepositoryFactory repositoryFactory) : base(repositoryFactory)
        {
        }
        public IList<Product> GetAll()
        {
            return GetAll(null);
        }

        public IList<Product> GetAll(Expression<Func<Product, bool>> conditionFunc)
        {
            var productRepository = RepositoryFactory.GetRepository<IProductRepository>();
            IList<Product> products;
            if (conditionFunc != null)
            {
                products = productRepository.GetAll().AsQueryable().Where(conditionFunc).ToList();
            }
            return productRepository.GetAll();
        }
    }
}
