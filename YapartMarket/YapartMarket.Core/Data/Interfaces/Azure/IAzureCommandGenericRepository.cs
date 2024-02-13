using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace YapartMarket.Core.Data.Interfaces.Azure
{
    public interface IAzureCommandGenericRepository<T> where T : class
    {
        Task InsertAsync(string sql, IEnumerable<object> inserts);
        Task<IEnumerable<int>> InsertAsync(IEnumerable<object> listObjects);
        Task InsertAsync(object @object);
        Task InsertAsync(string sql, object insert);
        Task<IEnumerable<int>> InsertOutputAsync(string sql, IEnumerable<object> inserts);
        Task UpdateAsync(string sql, object action);
        Task UpdateAsync(object action);
        Task DeleteAsync(string sql);
        DataTable ConvertToDataTable(IEnumerable<T> data);
    }
}
