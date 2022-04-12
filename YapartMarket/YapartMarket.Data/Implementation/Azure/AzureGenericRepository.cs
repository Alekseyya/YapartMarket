using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Dapper;
using Dapper.Contrib.Extensions;
using YapartMarket.Core.Data.Interfaces.Azure;
using YapartMarket.Core.DateStructures;
using YapartMarket.Core.Extensions;

namespace YapartMarket.Data.Implementation.Azure
{
    public abstract class AzureGenericRepository<T> : IAzureQueriesGenericRepository<T>, IAzureCommandGenericRepository<T> where T : class
    {
        private readonly string _tableName;
        private readonly string _connectionString;

        protected AzureGenericRepository(string tableName, string connectionString)
        {
            _tableName = tableName;
            _connectionString = connectionString;
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                return await connection.QueryAsync<T>($"select * from {_tableName}");
            }
        }

        public async Task<IEnumerable<T>> GetInAsync(string field, object action)
        {
            if (string.IsNullOrEmpty(field))
                throw new ArgumentException(field);
            if (action == null)
                throw new ArgumentNullException(nameof(action));
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();
                    return await connection.QueryAsync<T>($"select * from {_tableName} where {field} IN @{field}", action);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        private IList<string> GetSqlsInBatches(IList<string> userNames)
        {
            var insertSql = "INSERT INTO [Users] (Name, LastUpdatedAt) VALUES ";
            var valuesSql = "('{0}', getdate())";
            var batchSize = 1000;

            var sqlsToExecute = new List<string>();
            var numberOfBatches = (int)Math.Ceiling((double)userNames.Count / batchSize);

            for (int i = 0; i < numberOfBatches; i++)
            {
                var userToInsert = userNames.Skip(i * batchSize).Take(batchSize);
                var valuesToInsert = userToInsert.Select(u => string.Format(valuesSql, u));
                sqlsToExecute.Add(insertSql + string.Join(',', valuesToInsert));
            }

            return sqlsToExecute;
        }

        /// <summary>
        /// Получить записи по sql запросу
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public async Task<IEnumerable<T>> GetAsync(string sql)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                return await connection.QueryAsync<T>(sql);
            }
        }

        public async Task<IEnumerable<T>> GetAsync(string sql, DynamicParameters dynamicParameters)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                return await connection.QueryAsync<T>(sql, dynamicParameters);
            }
        }

        public async Task<IEnumerable<T>> GetAsync(string sql, object action)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                return await connection.QueryAsync<T>(sql, action);
            }
        }

        public async Task<T> GetById(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                return await connection.QueryFirstAsync<T>($"select * from {_tableName} where id = {id}");
            }
        }

        //public virtual async Task<IEnumerable<int>> InsertAsync(string sql, IEnumerable<object> inserts)
        //{
        //    using (var connection = new SqlConnection(_connectionString))
        //    {
        //        await connection.OpenAsync();
        //        return await connection.QueryAsync<int>(sql, inserts);
        //    }
        //}
        
        public virtual async Task InsertAsync(string sql, IEnumerable<object> inserts)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();
                    foreach (var insert in inserts)
                    {
                        await connection.ExecuteAsync(sql, insert);
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            
        }

        public virtual async Task InsertAsync(string sql, object insert)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();

                    await connection.ExecuteAsync(sql, insert);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public virtual async Task<IEnumerable<int>> InsertAsync(IEnumerable<object> listObjects)
        {
            var insertSql = Activator.CreateInstance<T>().InsertString(_tableName);
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                return await connection.QueryAsync<int>(insertSql, listObjects);
            }
        }

        public virtual async Task InsertAsync(object @object)
        {
            var insertSql = Activator.CreateInstance<T>().InsertString(_tableName);
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                await connection.QueryAsync(insertSql, @object);
            }
        }

        public virtual async Task<IEnumerable<int>> InsertOutputAsync(string sql, IEnumerable<object> inserts)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                return await connection.QueryAsync<int>(sql, inserts);
            }
        }

        public virtual async Task Update(string sql, object action)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                await connection.ExecuteAsync(sql, action);
            }
        }

        public virtual async Task Update(object action)
        {
            var updateSQL = Activator.CreateInstance<T>().UpdateString(_tableName);
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                await connection.ExecuteAsync(updateSQL, action);
            }
        }
    }
}
