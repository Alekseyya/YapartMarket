using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;

namespace YapartMarket.Core.Extensions
{
    public static class AccessExtension
    {
        public static IEnumerable<PropertyInfo> GetProperties<T>()
        {
            return typeof(T).GetProperties();
        }
        public static List<string> GenerateListOfProperties(IEnumerable<PropertyInfo> listOfProperties)
        {
            return (from prop in listOfProperties
                let attributes = prop.GetCustomAttributes(typeof(DescriptionAttribute), false)
                where attributes.Length <= 0 || (attributes[0] as DescriptionAttribute)?.Description != "ignore"
                select prop.Name).ToList();
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
