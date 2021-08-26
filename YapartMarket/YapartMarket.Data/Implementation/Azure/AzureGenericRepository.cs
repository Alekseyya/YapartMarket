﻿using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Dapper;
using YapartMarket.Core.Data.Interfaces.Azure;

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
                connection.Open();
                return await connection.QueryAsync<T>($"select * from {_tableName}");
            }
        }

        public async Task<IEnumerable<T>> GetInAsync(string field, object action)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                return await connection.QueryAsync<T>($"select * from {_tableName} where {field} IN @{field}", action);
            }
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
                connection.Open();
                return await connection.QueryAsync<T>(sql);
            }
        }

        public Task<T> GetById(int id)
        {
            throw new NotImplementedException();
        }

        public async Task InsertAsync(string sql, Action<object> action)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                await connection.ExecuteAsync(sql, action);
            }
        }

        public async Task Update(string sql, Action<object> action)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                await connection.ExecuteAsync(sql, action);
            }
        }
    }
}
