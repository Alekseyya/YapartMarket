using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using YapartMarket.Core.Data.Interfaces;
using YapartMarket.Core.Extensions;

namespace YapartMarket.Data.Implementation.Access
{
    public abstract class AccessGenericRepository<T> : IAccessGenericRepository<T> where T : class
    {
        private readonly string _connectionString;
        private readonly string _tableName;

        protected AccessGenericRepository(string tableName, string connectionString)
        {
            _tableName = tableName;
        }
        public async Task<IEnumerable<T>> GetAllAsync()
        {
            using (var connection  = new OleDbConnection(_connectionString))
            {
                connection.Open();
                return await connection.QueryAsync<T>($"select * from {_tableName}");
            }
        }

        public async Task<List<T>> GetAsync(string sql)
        {
            using (var connection = new OleDbConnection(_connectionString))
            {
                connection.Open();
                var result = await connection.QueryAsync<T>(sql);
                return result.ToList();
            }
        }

        public async Task InsertAsync(T t)
        {
            var insertQuery = GenerateInsertQuery();
            using (var connection = new OleDbConnection(_connectionString))
            {
                connection.Open();
                await connection.ExecuteAsync(insertQuery, t);
            }
        }

        public async Task Update(T t)
        {
            var updateQuery = GenerateUpdateQuery();
            using (var connection = new OleDbConnection(_connectionString))
            {
                await connection.ExecuteAsync(updateQuery, t);
            }
        }

        public string GenerateUpdateQuery()
        {
            var updateQuery = new StringBuilder($"UPDATE {_tableName} SET ");
            var properites = AccessExtension.GetProperties<T>();
            var idProperty = AccessExtension.GetKeyProperty<T>();
            properites.ForEach(prop =>
            {
                if (!prop.Equals(idProperty))
                {
                    updateQuery.Append($"{prop}=@{prop},");
                }
            });
            updateQuery.Remove(updateQuery.Length - 1, 1); //Удалить последнюю запятую
            updateQuery.Append($" WHERE {idProperty}=@{idProperty}");
            return updateQuery.ToString();
        }

        public string GenerateInsertQuery()
        {
            var insertQuery = new StringBuilder($"INSERT INTO {_tableName}");
            insertQuery.Append("(");
            var properties = AccessExtension.GetProperties<T>();
            properties.ForEach(prop => { insertQuery.Append($"{prop},"); });
            //Удалить последнюю запятую
            insertQuery.Remove(insertQuery.Length - 1, 1).Append(") VALUES (");
            properties.ForEach(prop => { insertQuery.Append($"@{prop},"); });
            //Удалить последнюю запятую
            insertQuery.Remove(insertQuery.Length - 1, 1).Append(")");
            return insertQuery.ToString();
        }

        public async Task<T> GetById(int id)
        {
            var idProperty = AccessExtension.GetKeyProperty<T>();
            if (!string.IsNullOrEmpty(idProperty))
            {
                using (var connection = new OleDbConnection(_connectionString))
                {
                    connection.Open();
                    return await connection.QueryFirstAsync<T>($"select * from {_tableName} where {idProperty} = {id}");
                }
            }
            //Todo хорошо  это или плохо, надо погуглить
            return null;
        }
    }
}
