using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace YapartMarket.Core.Data
{
    //Todo если не получаиться то исправить
    public interface IYapartDbAccessor : IDbAccessor<DbContext>, IUnitOfWork
    {
    }
}
