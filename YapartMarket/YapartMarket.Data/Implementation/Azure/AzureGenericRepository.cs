﻿using System;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Dapper;
using Dapper.Contrib.Extensions;
using YapartMarket.Core.Extensions;
using YapartMarket.Core.Data.Interfaces.Azure;

namespace YapartMarket.Data.Implementation.Azure
{
    public abstract class AzureGenericRepository<T> : IAzureQueriesGenericRepository<T>, IAzureCommandGenericRepository<T> where T : class
    {
        readonly string tableName;
        readonly string connectionString;

        protected AzureGenericRepository(string tableName, string connectionString)
        {
            this.tableName = tableName ?? throw new ArgumentNullException(nameof(tableName));
            this.connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            using (var connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();
                return await connection.QueryAsync<T>($"select * from {tableName}");
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
                using (var connection = new SqlConnection(connectionString))
                {
                    await connection.OpenAsync();
                    return await connection.QueryAsync<T>($"select * from {tableName} where {field} IN @{field}", action);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        public DataTable ConvertToDataTable(IEnumerable<T> data)
        {
            var properties = typeof(T).GetProperties();
            DataTable table = new DataTable();
            foreach (var property in properties)
            {
                if (property.PropertyType.IsClass && property.PropertyType != typeof(string))
                    continue;

                // Skip methods without a public setter
                if (property.GetSetMethod() == null)
                    continue;

                // Skip methods specifically ignored
                if (property.IsDefined(typeof(ComputedAttribute), false))
                    continue;
                if (property.PropertyType.IsGenericType && property.PropertyType.GetGenericTypeDefinition() == typeof(List<>))
                    continue;
                var name = property.GetCustomAttribute<ColumnAttribute>()?.Name;
                if (!string.IsNullOrEmpty(name))
                    table.Columns.Add(name, Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType);
            }
                
            foreach (T item in data)
            {
                DataRow row = table.NewRow();
                foreach (var property in properties)
                {
                    if (property.PropertyType.IsClass && property.PropertyType != typeof(string))
                        continue;

                    // Skip methods without a public setter
                    if (property.GetSetMethod() == null)
                        continue;

                    // Skip methods specifically ignored
                    if (property.IsDefined(typeof(ComputedAttribute), false))
                        continue;
                    if (property.PropertyType.IsGenericType && property.PropertyType.GetGenericTypeDefinition() == typeof(List<>))
                        continue;
                    var name = property.GetCustomAttribute<ColumnAttribute>()?.Name;
                    if(!string.IsNullOrEmpty(name))
                        row[name] = property.GetValue(item) ?? DBNull.Value;
                }
                table.Rows.Add(row);
            }
            return table;
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
            using (var connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();
                return await connection.QueryAsync<T>(sql);
            }
        }

        public async Task<IEnumerable<T>> GetAsync(string sql, DynamicParameters dynamicParameters)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();
                return await connection.QueryAsync<T>(sql, dynamicParameters);
            }
        }

        public async Task<IEnumerable<T>> GetAsync(string sql, object action)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();
                return await connection.QueryAsync<T>(sql, action);
            }
        }

        public async Task<T> GetByIdAsync(int id)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();
                return await connection.QueryFirstAsync<T>($"select * from {tableName} where id = {id}");
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
                using (var connection = new SqlConnection(connectionString))
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
                using (var connection = new SqlConnection(connectionString))
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
            var insertSql = Activator.CreateInstance<T>().InsertString(tableName);
            using (var connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();
                return await connection.QueryAsync<int>(insertSql, listObjects);
            }
        }

        public virtual async Task InsertAsync(object @object)
        {
            var insertSql = Activator.CreateInstance<T>().InsertString(tableName);
            using (var connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();
                await connection.QueryAsync(insertSql, @object);
            }
        }

        public virtual async Task<IEnumerable<int>> InsertOutputAsync(string sql, IEnumerable<object> inserts)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();
                return await connection.QueryAsync<int>(sql, inserts);
            }
        }

        public virtual async Task UpdateAsync(string sql, object action)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();
                await connection.ExecuteAsync(sql, action);
            }
        }

        public virtual async Task DeleteAsync(string sql)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();
                await connection.ExecuteAsync(sql);
            }
        }

        public virtual async Task UpdateAsync(object action)
        {
            var updateSQL = Activator.CreateInstance<T>().UpdateString(tableName);
            using (var connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();
                await connection.ExecuteAsync(updateSQL, action);
            }
        }
    }
}
