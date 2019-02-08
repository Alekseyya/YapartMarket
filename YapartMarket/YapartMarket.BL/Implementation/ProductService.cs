using System;
using System.Collections.Generic;
using System.Text;
using YapartMarket.Core.BL;
using YapartMarket.Core.Data;

namespace YapartMarket.BL.Implementation
{
    public class ProductService : RepositoryAwareServiceBase, IProductService
    {
        public ProductService(IRepositoryFactory repositoryFactory) : base(repositoryFactory)
        {
        }
    }
}
