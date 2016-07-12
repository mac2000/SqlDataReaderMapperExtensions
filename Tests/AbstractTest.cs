using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Smo;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tests.Models;

namespace Tests
{
    public abstract class AbstractTest
    {
        protected readonly List<Post> Data = new List<Post>
            {
                new Post
                {
                    Id = 1,
                    Title = "Post 1",
                    CreatedAt = DateTime.Now.Date,
                    Published = true,
                    PublishedAt = DateTime.Now.Date,
                    ListInt = new List<int> { 1 },
                    ListString = new List<string> { "Tag 1" },
                    ListTag = new List<Tag> { new Tag { Id = 1, Name = "Tag 1" } }
                },
                new Post
                {
                    Id = 2,
                    Title = "Post 2",
                    CreatedAt = DateTime.Now.Date,
                    Published = false,
                    ListInt = new List<int> { 1, 2 },
                    ListString = new List<string> { "Tag 1", "Tag 2" },
                    ListTag = new List<Tag> { new Tag { Id = 1, Name = "Tag 1" }, new Tag { Id = 2, Name = "Tag 2" } }
                },
                new Post
                {
                    Id = 3,
                    Title = "Post 3",
                    CreatedAt = DateTime.Now.Date,
                    Published = true,
                    PublishedAt = DateTime.Now.Date,
                    ListInt = new List<int>(),
                    ListString = new List<string>(),
                    ListTag = new List<Tag>()
                }
            };

        [AssemblyInitialize]
        public static void AssemblyInitialize(TestContext context)
        {
            if (IsAppVeyor) return;

            var server = new Server(new ServerConnection(GetSqlConnection("master")));
            server.ConnectionContext.ExecuteNonQuery(File.ReadAllText("Setup.sql"));
        }

        protected static bool IsAppVeyor => !string.IsNullOrEmpty(Environment.GetEnvironmentVariable("APPVEYOR"));

        protected static SqlConnection GetSqlConnection(string database = "SqlDataReaderMapperExtensionTests")
        {
            var connectionString = IsAppVeyor
                ? $"Server=(local)\\SQL2014;Database={database};User ID=sa;Password=Password12!"
                : $"Server=(localdb)\\MSSQLLocalDB;Database={database};Integrated Security=true";

            return new SqlConnection(connectionString);
        }

        protected SqlCommand GetSqlCommand()
        {
            return new SqlCommand("SELECT * FROM SampleView", GetSqlConnection());
        }
    }
}
