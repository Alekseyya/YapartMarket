using System;
using System.Collections.Generic;
using System.Text;
using YapartMarket.Core.BL;
using YapartMarket.Core.Data;

namespace YapartMarket.BL.Implementation
{
   public class MarkService : RepositoryAwareServiceBase, IMarkService
    {
        public MarkService(IRepositoryFactory repositoryFactory) : base(repositoryFactory)
        {
        }
    }
}
