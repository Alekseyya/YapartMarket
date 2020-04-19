using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using YapartMarket.Core.Models;

namespace YapartMarket.Core.BL
{
   public interface IModificationService
    {
        Modification Add(Modification modification);
        Modification Update(Modification modification);
        Modification GetById(int id);
        IList<Modification> GetAll();
        IList<Modification> GetAll(Expression<Func<Modification, bool>> conditionFunc);
    }
}
