using System.Collections.Generic;
using System.Threading.Tasks;

namespace YapartMarket.Core.Data.Interfaces
{
    public interface IAccessGenericRepository<T>
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<List<T>> GetAsync(string sql);
        Task<T> GetById(int id);
        Task InsertAsync(T t);
        Task Update(T t);
        string GenerateInsertQuery();
    }
}
