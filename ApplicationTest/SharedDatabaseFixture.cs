﻿using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Repository;

namespace ApplicationTest
{
    public class SharedDatabaseFixture : IDisposable
    {
        private readonly SqliteConnection connection;
        public SharedDatabaseFixture()
        {
            this.connection = new SqliteConnection("DataSource=:memory:");
            this.connection.Open();
        }
        public void Dispose() => this.connection.Dispose();
        public AppDatabaseContext CreateContext()
        {
            var result = new AppDatabaseContext(new DbContextOptionsBuilder<AppDatabaseContext>()
                .UseSqlite(this.connection)
                .Options
                );
            result.Database.EnsureCreated();
            
            return result;
        }
    }
}
