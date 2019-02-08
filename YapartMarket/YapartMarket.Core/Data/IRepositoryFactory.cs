using System;
using System.Collections.Generic;
using System.Text;

namespace YapartMarket.Core.Data
{
   public interface IRepositoryFactory
    {
        IRepository GetRepository<IRepository>();
    }
}
