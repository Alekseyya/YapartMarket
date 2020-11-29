using System.Collections.Generic;
using System.Threading.Tasks;
using YapartMarket.Core.AccessModels;

namespace YapartMarket.Core.Data.Interfaces.Access
{
    public interface IAccessProductRepository<T> where T : AccessProduct
    {
        Task<List<T>> Get();
        List<AccessProduct> GetInnerJoin();
    }
}
