using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace YapartMarket.Core.Data.Interfaces.Azure
{
    public interface IAzureQueriesGenericRepository<T>
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<IEnumerable<T>> GetInAsync(string field, object action);
        Task<IEnumerable<T>> GetAsync(string sql);
        Task<T> GetById(int id);
    }
}
