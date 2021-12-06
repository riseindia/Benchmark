using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using System.Dynamic;
using Core;

namespace Benchmark.ADONet
{
    public class PureAdoExecuter : IExecuter
    {
        SqlConnection conn;
        public static string ExecName
        {
            get
            {
                return "ADO (Pure)";
            }
        }
        public string Name
        {
            get
            {
                return "ADO (Pure)";
            }
        }

        public void Init(string connectionStrong)
        {
            conn = new SqlConnection(connectionStrong);
            conn.Open();
        }
        public ITest GetItemAsObject(int Id)
        {
            var cmd = conn.CreateCommand();
            if (conn.State != System.Data.ConnectionState.Open)
                conn.Open();
            cmd.CommandText = @"select * from Tests where Id = @Id";
            var idParam = cmd.Parameters.Add("@Id", System.Data.SqlDbType.Int);
            idParam.Value = Id;

            Test obj;
            using (var reader = cmd.ExecuteReader())
            {
                reader.Read();
                obj = new Test
                {
                    Id = reader.GetInt32(0),
                    Text = reader.GetNullableString(1),
                    CreationDate = reader.GetDateTime(2),
                    LastChangeDate = reader.GetDateTime(3),
                    Counter1 = reader.GetNullableValue<int>(4),
                    Counter2 = reader.GetNullableValue<int>(5),
                    Counter3 = reader.GetNullableValue<int>(6),
                    Counter4 = reader.GetNullableValue<int>(7),
                    Counter5 = reader.GetNullableValue<int>(8),
                    Counter6 = reader.GetNullableValue<int>(9),
                    Counter7 = reader.GetNullableValue<int>(10),
                    Counter8 = reader.GetNullableValue<int>(11),
                    Counter9 = reader.GetNullableValue<int>(12),
                };
            }

            return obj;
        }


        public IList<ITest> GetAllItemsAsObject()
        {
            var cmd = conn.CreateCommand();
            cmd.CommandText = @"select * from Tests";

            List<ITest> list = new List<ITest>();
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    Test obj = new Test
                    {
                        Id = reader.GetInt32(0),
                        Text = reader.GetNullableString(1),
                        CreationDate = reader.GetDateTime(2),
                        LastChangeDate = reader.GetDateTime(3),
                        Counter1 = reader.GetNullableValue<int>(4),
                        Counter2 = reader.GetNullableValue<int>(5),
                        Counter3 = reader.GetNullableValue<int>(6),
                        Counter4 = reader.GetNullableValue<int>(7),
                        Counter5 = reader.GetNullableValue<int>(8),
                        Counter6 = reader.GetNullableValue<int>(9),
                        Counter7 = reader.GetNullableValue<int>(10),
                        Counter8 = reader.GetNullableValue<int>(11),
                        Counter9 = reader.GetNullableValue<int>(12),
                    };

                    list.Add(obj);
                }
            }

            return list;
        }


        public void Finish()
        {
            conn.Close();
        }

        public void WriteOnce(int recordCount)
        {

            var cmd = conn.CreateCommand();
            if (conn.State != System.Data.ConnectionState.Open)
                conn.Open();

            cmd.CommandTimeout = 300;
            cmd.CommandText = string.Format(@"
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

            cmd.Connection = conn;
            cmd.ExecuteNonQuery();
            conn.Close();


        }
        public void WriteInLoop(int recordCount)
        {

            var cmd = conn.CreateCommand();
            if (conn.State != System.Data.ConnectionState.Open)
                conn.Open();
            cmd.CommandTimeout = 300;
            cmd.CommandText = @"
                    truncate table Tests";
            cmd.Connection = conn;
            cmd.ExecuteNonQuery();
            for (int i = 1; i <= recordCount; i++)
            {
                cmd.CommandText = string.Format(@"

                    set nocount on


                            insert Tests([Text], CreationDate, LastChangeDate) values(replicate('x', 2000), GETDATE(), GETDATE())


	                    
                    ", recordCount);

                cmd.Connection = conn;
                cmd.ExecuteNonQuery();
            }
            conn.Close();


        }
    }
}
