using System;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace YapartMarket.Core.Extensions
{
    public static class JsonObjectExtension
    {
        public static bool TryParseJson<T>(this string @this, out T result)
        {
            bool success = true;
            var settings = new JsonSerializerSettings
            {
                Error = (sender, args) => { success = false; args.ErrorContext.Handled = true; },
                MissingMemberHandling = MissingMemberHandling.Error
            };
            result = JsonConvert.DeserializeObject<T>(@this, settings);
            return success;
        }

        public static T FillProperties<T>(this T entity, JObject jObject) where T : class
        {
            foreach (var property in jObject.Properties())
            {
                foreach (var fieldProperty in entity.GetType().GetProperties())
                {
                    if (fieldProperty.GetCustomAttributes(true).Cast<JsonPropertyAttribute>().FirstOrDefault()?.PropertyName == property.Name)
                    {
                        fieldProperty.SetValue(entity, Convert.ChangeType(property.Value, fieldProperty.PropertyType), null);
                    }
                }
            }
            return entity;
        }
    }
}
