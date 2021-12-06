using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Core
{

    public class Benchmarker
    {
        private List<IExecuter> executers { get; set; }
        public List<BenchmarkResult> singleReadresults { get; set; }
        public List<BenchmarkResult> combinedWriteResults { get; set; }
        public List<BenchmarkResult> singleWriteResults { get; set; }
        public List<BenchmarkResult> combinedReadresultsForAllItems { get; set; }
        private int IterationCount { get; set; }
        private string ConnectionString { get; set; }

        public Benchmarker(string connectionString, int iterationCount)
        {
            ConnectionString = connectionString;
            IterationCount = iterationCount;
            executers = new List<IExecuter>();
            singleReadresults = new List<BenchmarkResult>();
            combinedReadresultsForAllItems = new List<BenchmarkResult>();
            combinedWriteResults = new List<BenchmarkResult>();
            singleWriteResults = new List<BenchmarkResult>();
        }

        public void RegisterOrmExecuter(IExecuter executer)
        {
            executers.Add(executer);
        }

        public void Run()
        {
            PrepareDatabase();

            singleReadresults.Clear();
            combinedWriteResults.Clear();
            singleWriteResults.Clear();
            combinedReadresultsForAllItems.Clear();

            var rand = new Random();
            foreach (IExecuter executer in executers.OrderBy(ignore => rand.Next()))
            {
                executer.Init(ConnectionString);


                try
                {

                    Stopwatch watchForWritingAllItems = new Stopwatch();
                    watchForWritingAllItems.Start();
                    executer.WriteOnce(IterationCount);
                    watchForWritingAllItems.Stop();
                    combinedWriteResults.Add(new BenchmarkResult { Name = executer.Name, ExecTime = watchForWritingAllItems.ElapsedMilliseconds });

                    Stopwatch watchForWritingSingleItems = new Stopwatch();
                    watchForWritingSingleItems.Start();
                    executer.WriteInLoop(IterationCount);
                    watchForWritingSingleItems.Stop();
                    singleWriteResults.Add(new BenchmarkResult { Name = executer.Name, ExecTime = watchForWritingSingleItems.ElapsedMilliseconds });

                }
                catch (Exception e1)
                {

                }
                Stopwatch watch = new Stopwatch();


                long firstItemExecTime = 0;
                for (int i = 1; i <= IterationCount; i++)
                {
                    watch.Start();
                    var obj = executer.GetItemAsObject(i);
                    watch.Stop();
                    if (i == 1)
                        firstItemExecTime = watch.ElapsedMilliseconds;
                }
                singleReadresults.Add(new BenchmarkResult { Name = executer.Name, ExecTime = watch.ElapsedMilliseconds, FirstItemExecTime = firstItemExecTime });

                Stopwatch watchForAllItems = new Stopwatch();
                watchForAllItems.Start();
                executer.GetAllItemsAsObject();
                watchForAllItems.Stop();
                combinedReadresultsForAllItems.Add(new BenchmarkResult { Name = executer.Name, ExecTime = watchForAllItems.ElapsedMilliseconds });


                executer.Finish();
            }
        }

        private void PrepareDatabase()
        {
            using var conn = new SqlConnection(ConnectionString);
            conn.Open();
            var cmd = conn.CreateCommand();
            cmd.CommandTimeout = 300;
            cmd.CommandText = @"
                    if (OBJECT_ID('Tests') is null)
                    begin
	                    create table Tests
	                    (
		                    Id int IDENTITY(1,1) NOT NULL primary key, 
		                    [Text] varchar(max) not null, 
		                    CreationDate datetime not null, 
		                    LastChangeDate datetime not null,
		                    Counter1 int,
		                    Counter2 int,
		                    Counter3 int,
		                    Counter4 int,
		                    Counter5 int,
		                    Counter6 int,
		                    Counter7 int,
		                    Counter8 int,
		                    Counter9 int
	                    )
	                    end                  
	                    
                    ";

            cmd.Connection = conn;
            cmd.ExecuteNonQuery();
            conn.Close();
        }
    }
}

