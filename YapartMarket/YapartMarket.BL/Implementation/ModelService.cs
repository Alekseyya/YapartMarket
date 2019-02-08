using System;
using System.Collections.Generic;
using System.Text;
using YapartMarket.Core.BL;
using YapartMarket.Core.Data;

namespace YapartMarket.BL.Implementation
{
   public class ModelService : RepositoryAwareServiceBase, IModelService
    {
        public ModelService(IRepositoryFactory repositoryFactory) : base(repositoryFactory)
        {
        }
    }
}
