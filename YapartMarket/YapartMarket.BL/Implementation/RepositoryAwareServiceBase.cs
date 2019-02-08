using System;
using YapartMarket.Core.Data;

namespace YapartMarket.BL.Implementation
{
   public abstract class RepositoryAwareServiceBase
    {
        protected IRepositoryFactory RepositoryFactory { get; }

        protected RepositoryAwareServiceBase(IRepositoryFactory repositoryFactory)
        {
            if (repositoryFactory == null)
                throw new ArgumentNullException(nameof(repositoryFactory));

            RepositoryFactory = repositoryFactory;
        }
    }
}
