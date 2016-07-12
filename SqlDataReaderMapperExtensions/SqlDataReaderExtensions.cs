using System;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Linq;
using System.Reflection;
using System.Runtime.Caching;
using System.Xml.Serialization;

namespace SqlDataReaderMapperExtensions
{
    public static class SqlDataReaderExtensions
    {
        public static T Map<T>(this SqlDataReader reader) where T : new()
        {
            var properties = GetProperties(typeof(T));

            var item = new T();

            for (var i = 0; i < reader.FieldCount; i++)
            {
                var property = properties[NormalizeKey(reader.GetName(i))].FirstOrDefault();

                if (property == null || reader.IsDBNull(i)) continue;

                if (reader.GetFieldType(i) == property.PropertyType)
                {
                    property.SetValue(item, reader[i]);
                }
                else if (reader.GetProviderSpecificFieldType(i) == typeof(SqlXml))
                {
                    var serializer = new XmlSerializer(property.PropertyType);
                    property.SetValue(item, serializer.Deserialize(reader.GetXmlReader(i)));
                }
            }

            return item;
        }

        private static readonly MemoryCache Cache = MemoryCache.Default;

        private static ILookup<string, PropertyInfo> GetProperties(Type type)
        {
            var cacheKey = $"{nameof(SqlDataReaderMapperExtensions)}.{type.FullName}";
            var result = Cache.Get(cacheKey) as ILookup<string, PropertyInfo>;

            if (result != null) return result;

            result = type.GetProperties(BindingFlags.Public | BindingFlags.Instance).Where(p => p.CanWrite).ToLookup(p => NormalizeKey(p.Name));
            Cache.Add(cacheKey, result, null);

            return result;
        }

        private static string NormalizeKey(string name)
        {
            return name.Replace("_", "").ToLower().Trim();
        }
    }
}