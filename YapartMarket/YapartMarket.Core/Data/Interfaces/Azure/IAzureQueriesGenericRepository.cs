using System.Collections.Generic;
using System.Threading.Tasks;
//using Dapper;

namespace YapartMarket.Core.Data.Interfaces.Azure
{
    public interface IAzureQueriesGenericRepository<T>
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<IEnumerable<T>> GetInAsync(string field, object action);
        Task<IEnumerable<T>> GetAsync(string sql);
        Task<IEnumerable<T>> GetAsync(string sql, object action);
        //Task<IEnumerable<T>> GetAsync(string sql, DynamicParameters dynamicParameters);
        Task<T> GetByIdAsync(int id);
    }
}
