using System.Collections.Generic;
using System.Threading.Tasks;
using YapartMarket.Core.AccessModels;

namespace YapartMarket.Core.Data.Interfaces.Access
{
    public interface IAccessProductTypeRepository<T> where  T : AccessProductType
    {
        Task<List<T>> Get();
        List<T> GetInnerJoin();
    }
}
