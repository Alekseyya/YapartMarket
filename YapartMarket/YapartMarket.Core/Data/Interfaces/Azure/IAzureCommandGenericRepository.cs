using System;
using System.Threading.Tasks;

namespace YapartMarket.Core.Data.Interfaces.Azure
{
    public interface IAzureCommandGenericRepository<T>
    {
        Task InsertAsync(string sql, Action<object> action);
        Task Update(string sql, Action<object> action);
    }
}
