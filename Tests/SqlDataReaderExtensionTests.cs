using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Smo;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SqlDataReaderMapperExtensions;
using Tests.Models;

namespace Tests
{
    [TestClass]
    public class SqlDataReaderExtensionTests : AbstractTest
    {
        [TestMethod]
        public void TestGenericMapExtension()
        {
            var actual = new List<Post>();

            using (var command = GetSqlCommand())
            {
                command.Connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            var post = reader.Map<Post>();
                            actual.Add(post);
                        }
                    }
                }
                command.Connection.Close();
            }

            CollectionAssert.AreEqual(Data, actual);
        }
    }
}
