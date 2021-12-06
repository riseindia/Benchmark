using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Text;

namespace Benchmark.EF
{
    class TestContext : DbContext
    {
        private string ConnectionString;

        public TestContext(string connectionString) : base(connectionString)
        {
            ConnectionString = connectionString;
        }

        public DbSet<Test> Tests { get; set; }

    }
}