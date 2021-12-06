using Core;
using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Benchmark.Dapper
{
    public class DapperContribExecuter : IExecuter
    {
        SqlConnection conn;
        public static string ExecName
        {
            get
            {
                return "Dapper Contrib";
            }
        }
        public string Name
        {
            get
            {
                return "Dapper Contrib";
            }
        }

        public void Init(string connectionStrong)
        {
            conn = new SqlConnection(connectionStrong);
            conn.Open();
        }

        public ITest GetItemAsObject(int Id)
        {
            return conn.Get<Test>(Id);
        }


        public IList<ITest> GetAllItemsAsObject()
        {
            return conn.GetAll<Test>().ToList<ITest>();
        }


        public void Finish()
        {
            conn.Close();
        }


        public void WriteOnce(int recordCount)
        {
            var defaultColor = Console.ForegroundColor;

            var tests = new List<Test>();
            Stopwatch s = new Stopwatch();
            s.Start();
            for (int i = 0; i < recordCount; i++)
            {
                tests.Add(new Test { Text = "erere", CreationDate = DateTime.Now, LastChangeDate = DateTime.Now });
            }
            s.Stop();
            Console.ForegroundColor = ConsoleColor.DarkBlue;
            Console.WriteLine("Time taken in constructing object from " + Name + " is : " + s.ElapsedMilliseconds);
            Console.ForegroundColor = defaultColor;
            conn.Insert<List<Test>>(tests);

        }

        public void WriteInLoop(int recordCount)
        {

            conn.DeleteAll<Test>();

            for (int i = 0; i < recordCount; i++)
            {
                var post = new Test { Text = "erere", CreationDate = DateTime.Now, LastChangeDate = DateTime.Now };
                conn.Insert<Test>(post);
            }
        }
    }
}
