using System.Collections.Generic;
using System.Data.SqlClient;

namespace SqlDataReaderMapperExtensions
{
    public static class SqlCommandExtensions
    {
        public static IEnumerable<T> ExecuteReader<T>(this SqlCommand command) where T : new()
        {
            command.Connection.Open();
            using (var reader = command.ExecuteReader())
            {
                if (!reader.HasRows) yield break;

                while (reader.Read())
                {
                    yield return reader.Map<T>();
                }
            }
            command.Connection.Close();
        }
    }
}