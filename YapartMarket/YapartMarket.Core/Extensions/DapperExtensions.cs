using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection;
using Dapper.Contrib.Extensions;
using YapartMarket.Core.DateStructures;

namespace YapartMarket.Core.Extensions
{
    public static class DapperExtensions
    {
        public static string InsertString<T>(this T obj, string tableName/*, out IDictionary<string, object> valuePairs*/)
        {
            var propertyContainer = ParseProperties(obj);
            //valuePairs = propertyContainer.ValuePairs;
            var sql = $@"INSERT INTO {tableName} ({string.Join(", ", propertyContainer.ValueNames)}) 
            VALUES(@{string.Join(", @", propertyContainer.ValueNames)}) SELECT CAST(scope_identity() AS int)";
            return sql;
        }

        public static string UpdateString<T>(this T obj, string tableName)
        {
            var propertyContainer = ParseProperties(obj);
            var sqlIdPairs = GetSqlPairs(propertyContainer.IdNames);
            var sqlValuePairs = GetSqlPairs(propertyContainer.ValueNames);
            var sql = $@"UPDATE {tableName} 
            SET {sqlValuePairs}
            WHERE {sqlIdPairs}";
            return sql;
        }

        private static string GetSqlPairs(IEnumerable<string> keys, string separator = ", ")
        {
            var pairs = keys.Select(key => string.Format("{0}=@{0}", key)).ToList();
            return string.Join(separator, pairs);
        }


        private static PropertyContainer ParseProperties<T>(T obj)
        {
            var propertyContainer = new PropertyContainer();

            //var typeName = typeof(T).Name;
            var validKeyNames = new[] { "id", "Id" }; //string.Format("{0}Id", typeName), string.Format("{0}_Id", typeName) 

            var properties = typeof(T).GetProperties();
            foreach (var property in properties)
            {
                // Skip reference types (but still include string!)
                if (property.PropertyType.IsClass && property.PropertyType != typeof(string))
                    continue;

                // Skip methods without a public setter
                if (property.GetSetMethod() == null)
                    continue;

                // Skip methods specifically ignored
                if (property.IsDefined(typeof(ComputedAttribute), false))
                    continue;
                if(property.PropertyType.IsGenericType && property.PropertyType.GetGenericTypeDefinition() == typeof(List<>))
                    continue;

                var name = property.GetCustomAttribute<ColumnAttribute>()?.Name;
                if (!string.IsNullOrEmpty(name))
                {
                    var value = typeof(T).GetProperty(property.Name)?.GetValue(obj, null);


                    if (property.IsDefined(typeof(KeyAttribute), false) || validKeyNames.Contains(name))
                        propertyContainer.AddId(name, value);
                    else
                        propertyContainer.AddValue(name, value);
                }
            }
            return propertyContainer;
        }
    }
}
