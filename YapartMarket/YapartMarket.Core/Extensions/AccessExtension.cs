using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace YapartMarket.Core.Extensions
{
    public static class AccessExtension
    {
        public static List<string> GetProperties<T>()
        {
            var nameProperties = new List<string>();
            foreach (var property in typeof(T).GetProperties(BindingFlags.Instance | BindingFlags.Public))
            {
                if (property.CanRead && property.CanWrite)
                {
                    if (property.PropertyType == typeof(int) || property.PropertyType == typeof(string))
                    {
                       nameProperties.Add(property.Name);
                    }
                }
            }
            return nameProperties;
        }
        

        public static string GetKeyProperty<T>()
        {
            foreach (var prop in typeof(T).GetProperties())
            {
                if (prop.GetCustomAttribute(typeof(KeyAttribute), false) != null)
                    return prop.Name;
            }
            return null;
        }
    }
}
