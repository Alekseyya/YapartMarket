using System;
using System.Collections.Generic;
using System.Text;
using YapartMarket.Core.BL;
using YapartMarket.Core.Data;

namespace YapartMarket.BL.Implementation
{
    public class ProductModificationService : RepositoryAwareServiceBase, IProductModificationService
    {
        public ProductModificationService(IRepositoryFactory repositoryFactory) : base(repositoryFactory)
        {
        }
    }
}
