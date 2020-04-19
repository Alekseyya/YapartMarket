using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using YapartMarket.Core.Models;

namespace YapartMarket.Core.BL
{
    public interface IProductService
    {
        Product Add(Product product);
        Product Update(Product product);
        Product GetById(int id);
        IList<Product> GetAll();
        IList<Product> GetAll(Expression<Func<Product, bool>> conditionFunc);
    }
}
