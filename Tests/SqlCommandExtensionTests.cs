using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SqlDataReaderMapperExtensions;
using Tests.Models;

namespace Tests
{
    [TestClass]
    public class SqlCommandExtensionTests : AbstractTest
    {
        [TestMethod]
        public void TestGenericExecuteReaderExtension()
        {
            var actual = new List<Post>();

            using (var command = GetSqlCommand())
            {
                actual.AddRange(command.ExecuteReader<Post>());
            }

            CollectionAssert.AreEqual(Data, actual);
        }
    }
}
