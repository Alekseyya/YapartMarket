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
    public class ProductService : RepositoryAwareServiceBase, IProductService
    {
        public ProductService(IRepositoryFactory repositoryFactory) : base(repositoryFactory)
        {
        }

        public Product Add(Product product)
        {
            var productRepository = RepositoryFactory.GetRepository<IProductRepository>();
            productRepository.Add(product);
            return product;
        }

        public Product Update(Product product)
        {
            var productRepository = RepositoryFactory.GetRepository<IProductRepository>();
            productRepository.Update(product);
            return product;
        }

        public Product GetById(int id)
        {
            var productRepository = RepositoryFactory.GetRepository<IProductRepository>();
            var model = productRepository.GetById(id);
            return model;
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
