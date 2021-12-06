using Core;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace Benchmark.Dapper
{
    public class DapperExecuter : IExecuter
    {
        SqlConnection conn;
        public static string ExecName
        {
            get
            {
                return "Dapper Query (Non Buffered)";
            }
        }
        public string Name
        {
            get
            {
                return "Dapper Query (Non Buffered)";
            }
        }

        public void Init(string connectionStrong)
        {
            conn = new SqlConnection(connectionStrong);
            conn.Open();
        }

        public ITest GetItemAsObject(int Id)
        {
            object param = new { Id = Id };
            return conn.Query<Test>("select * from Tests where Id=@Id", param, buffered: false).First();
        }

        public dynamic GetItemAsDynamic(int Id)
        {
            object param = new { Id = Id };
            return conn.Query("select * from Tests where Id=@Id", param, buffered: false).First();
        }

        public IList<ITest> GetAllItemsAsObject()
        {
            return conn.Query<Test>("select * from Tests", null, buffered: false).ToList<ITest>();
        }

        public IList<dynamic> GetAllItemsAsDynamic()
        {
            return conn.Query("select * from Tests", null, buffered: false).ToList();
        }

        public void WriteOnce(int recordCount)
        {
            var query = string.Format(@"
                    truncate table Tests
                    set nocount on

                        declare @i int
                        declare @c int
                        declare @id int
                        set @i = 0


                        while @i <= {0}

                        begin
                            insert Tests([Text], CreationDate, LastChangeDate) values(replicate('x', 2000), GETDATE(), GETDATE())
		                    set @i = @i + 1


                        end               
	                    
                    ", recordCount);
            conn.Execute(query);
        }

        public void WriteInLoop(int recordCount)
        {
            var query1 = @"
                    truncate table Tests";
            conn.Execute(query1);

            for (int i = 0; i < recordCount; i++)
            {
                var query2 =
                    "insert Tests([Text], CreationDate, LastChangeDate) values(replicate('x', 2000), GETDATE(), GETDATE())";
                conn.Execute(query2);
            }
        }
        public void Finish()
        {
            conn.Close();
        }
    }
}
