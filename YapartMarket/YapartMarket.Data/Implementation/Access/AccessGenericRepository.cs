using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.OleDb;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using YapartMarket.Core.AccessModels;
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

        //Todo Для Insert взять с сайта https://itnext.io/generic-repository-pattern-using-dapper-bd48d9cd7ead

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
