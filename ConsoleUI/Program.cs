using System;

namespace ConsoleUI
{
    class Program
    {
        static void Main(string[] args)
        {
            // Set up configuration sources.
            var builder = new ConfigurationBuilder()
           .SetBasePath(Directory.GetCurrentDirectory())
           .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
           .AddEnvironmentVariables();

            IConfigurationRoot configuration = builder.Build();

            string connStringName = configuration.GetValue<string>("DefaultConnectionStringName");
            string connStr = configuration.GetConnectionString(connStringName);

            // Set up data directory
            string runDir = System.AppContext.BaseDirectory;
            //string runDir = AppDomain.CurrentDomain.BaseDirectory;
            connStr = connStr.Replace("|DataDirectory|", runDir.TrimEnd('\\'));
            Dictionary<int, Benchmarker> resultDic = new Dictionary<int, Benchmarker>();
            List<int> testCases = new List<int>
            {
                1,
                10,
                100,
                1000,
                10000, 
                //100000 
            };
            var ver = Assembly.GetEntryAssembly()?.GetCustomAttribute<TargetFrameworkAttribute>()?.FrameworkName;
            Console.WriteLine(ver);
            Console.WriteLine("Connection string: {0}", connStr);
            for (int i = 0; i < testCases.Count; i++)
            {
                int recordcount = testCases[i];
                var benchmarker = new Benchmarker(connStr, recordcount);

                benchmarker.RegisterOrmExecuter(new Ado.PureAdoExecuter());
                benchmarker.RegisterOrmExecuter(new Dapper.DapperExecuter());
                benchmarker.RegisterOrmExecuter(new Dapper.DapperBufferedExecuter());
                benchmarker.RegisterOrmExecuter(new Dapper.DapperFirstOrDefaultExecuter());
                benchmarker.RegisterOrmExecuter(new Dapper.DapperContribExecuter());
                benchmarker.RegisterOrmExecuter(new OrmBenchmark..EntityFrameworNoTrackingExecuter());

                Console.WriteLine("ORM Benchmark");

                Console.Write("\nRunning...");
                benchmarker.Run();
                resultDic.Add(recordcount, benchmarker);


            }
            Dictionary<string, Dictionary<int, Dictionary<string, long>>> formattedValues = new Dictionary<string, Dictionary<int, Dictionary<string, long>>>();
            formattedValues.Add("Single Write", new Dictionary<int, Dictionary<string, long>>());
            formattedValues.Add("Combined Write", new Dictionary<int, Dictionary<string, long>>());
            formattedValues.Add("Single Read", new Dictionary<int, Dictionary<string, long>>());
            formattedValues.Add("Combined Read", new Dictionary<int, Dictionary<string, long>>());
            var keyEnumerator = resultDic.Keys.GetEnumerator();
            keyEnumerator.MoveNext();

            for (int i = 0; i < resultDic.Keys.Count; i++)
            {

                var resVal = new Dictionary<string, long>();

                resVal.Add(Ado.PureAdoExecuter.ExecName, resultDic[keyEnumerator.Current].singleWriteResults.Where(x => x.Name == Ado.PureAdoExecuter.ExecName).Select(x => x.ExecTime).FirstOrDefault());
                resVal.Add(Dapper.DapperExecuter.ExecName, resultDic[keyEnumerator.Current].singleWriteResults.Where(x => x.Name == Dapper.DapperExecuter.ExecName).Select(x => x.ExecTime).FirstOrDefault());
                resVal.Add(Dapper.DapperBufferedExecuter.ExecName, resultDic[keyEnumerator.Current].singleWriteResults.Where(x => x.Name == Dapper.DapperBufferedExecuter.ExecName).Select(x => x.ExecTime).FirstOrDefault());
                resVal.Add(Dapper.DapperFirstOrDefaultExecuter.ExecName, resultDic[keyEnumerator.Current].singleWriteResults.Where(x => x.Name == Dapper.DapperFirstOrDefaultExecuter.ExecName).Select(x => x.ExecTime).FirstOrDefault());
                resVal.Add(Dapper.DapperContribExecuter.ExecName, resultDic[keyEnumerator.Current].singleWriteResults.Where(x => x.Name == Dapper.DapperContribExecuter.ExecName).Select(x => x.ExecTime).FirstOrDefault());
                resVal.Add(EntityFramework.EntityFrameworNoTrackingExecuter.ExecName, resultDic[keyEnumerator.Current].singleWriteResults.Where(x => x.Name == EntityFramework.EntityFrameworNoTrackingExecuter.ExecName).Select(x => x.ExecTime).FirstOrDefault());


                formattedValues["Single Write"].Add(keyEnumerator.Current, resVal);

                var resVal1 = new Dictionary<string, long>();

                resVal1.Add(Ado.PureAdoExecuter.ExecName, resultDic[keyEnumerator.Current].combinedWriteResults.Where(x => x.Name == Ado.PureAdoExecuter.ExecName).Select(x => x.ExecTime).FirstOrDefault());
                resVal1.Add(Dapper.DapperExecuter.ExecName, resultDic[keyEnumerator.Current].combinedWriteResults.Where(x => x.Name == Dapper.DapperExecuter.ExecName).Select(x => x.ExecTime).FirstOrDefault());
                resVal1.Add(Dapper.DapperBufferedExecuter.ExecName, resultDic[keyEnumerator.Current].combinedWriteResults.Where(x => x.Name == Dapper.DapperBufferedExecuter.ExecName).Select(x => x.ExecTime).FirstOrDefault());
                resVal1.Add(Dapper.DapperFirstOrDefaultExecuter.ExecName, resultDic[keyEnumerator.Current].combinedWriteResults.Where(x => x.Name == Dapper.DapperFirstOrDefaultExecuter.ExecName).Select(x => x.ExecTime).FirstOrDefault());
                resVal1.Add(Dapper.DapperContribExecuter.ExecName, resultDic[keyEnumerator.Current].combinedWriteResults.Where(x => x.Name == Dapper.DapperContribExecuter.ExecName).Select(x => x.ExecTime).FirstOrDefault());
                resVal1.Add(EntityFramework.EntityFrameworNoTrackingExecuter.ExecName, resultDic[keyEnumerator.Current].combinedWriteResults.Where(x => x.Name == EntityFramework.EntityFrameworNoTrackingExecuter.ExecName).Select(x => x.ExecTime).FirstOrDefault());


                formattedValues["Combined Write"].Add(keyEnumerator.Current, resVal1);

                var resVal2 = new Dictionary<string, long>();
                resVal2.Add(Ado.PureAdoExecuter.ExecName, resultDic[keyEnumerator.Current].singleReadresults.Where(x => x.Name == Ado.PureAdoExecuter.ExecName).Select(x => x.ExecTime).FirstOrDefault());
                resVal2.Add(Dapper.DapperExecuter.ExecName, resultDic[keyEnumerator.Current].singleReadresults.Where(x => x.Name == Dapper.DapperExecuter.ExecName).Select(x => x.ExecTime).FirstOrDefault());
                resVal2.Add(Dapper.DapperBufferedExecuter.ExecName, resultDic[keyEnumerator.Current].singleReadresults.Where(x => x.Name == Dapper.DapperBufferedExecuter.ExecName).Select(x => x.ExecTime).FirstOrDefault());
                resVal2.Add(Dapper.DapperFirstOrDefaultExecuter.ExecName, resultDic[keyEnumerator.Current].singleReadresults.Where(x => x.Name == Dapper.DapperFirstOrDefaultExecuter.ExecName).Select(x => x.ExecTime).FirstOrDefault());
                resVal2.Add(Dapper.DapperContribExecuter.ExecName, resultDic[keyEnumerator.Current].singleReadresults.Where(x => x.Name == Dapper.DapperContribExecuter.ExecName).Select(x => x.ExecTime).FirstOrDefault());
                resVal2.Add(EF1.EntityFrameworNoTrackingExecuter.ExecName, resultDic[keyEnumerator.Current].singleReadresults.Where(x => x.Name == EntityFramework.EntityFrameworNoTrackingExecuter.ExecName).Select(x => x.ExecTime).FirstOrDefault());

                formattedValues["Single Read"].Add(keyEnumerator.Current, resVal2);

                var resVal3 = new Dictionary<string, long>();
                resVal3.Add(Ado.PureAdoExecuter.ExecName, resultDic[keyEnumerator.Current].combinedReadresultsForAllItems.Where(x => x.Name == Ado.PureAdoExecuter.ExecName).Select(x => x.ExecTime).FirstOrDefault());
                resVal3.Add(Dapper.DapperExecuter.ExecName, resultDic[keyEnumerator.Current].combinedReadresultsForAllItems.Where(x => x.Name == Dapper.DapperExecuter.ExecName).Select(x => x.ExecTime).FirstOrDefault());
                resVal3.Add(Dapper.DapperBufferedExecuter.ExecName, resultDic[keyEnumerator.Current].combinedReadresultsForAllItems.Where(x => x.Name == Dapper.DapperBufferedExecuter.ExecName).Select(x => x.ExecTime).FirstOrDefault());
                resVal3.Add(Dapper.DapperFirstOrDefaultExecuter.ExecName, resultDic[keyEnumerator.Current].combinedReadresultsForAllItems.Where(x => x.Name == Dapper.DapperFirstOrDefaultExecuter.ExecName).Select(x => x.ExecTime).FirstOrDefault());
                resVal3.Add(Dapper.DapperContribExecuter.ExecName, resultDic[keyEnumerator.Current].combinedReadresultsForAllItems.Where(x => x.Name == Dapper.DapperContribExecuter.ExecName).Select(x => x.ExecTime).FirstOrDefault());
                resVal3.Add(EntityFramework.EntityFrameworNoTrackingExecuter.ExecName, resultDic[keyEnumerator.Current].combinedReadresultsForAllItems.Where(x => x.Name == EntityFramework.EntityFrameworNoTrackingExecuter.ExecName).Select(x => x.ExecTime).FirstOrDefault());
                formattedValues["Combined Read"].Add(keyEnumerator.Current, resVal3);
                keyEnumerator.MoveNext();
            }
            PrintResults(formattedValues);
            Console.WriteLine("Finished.");

            Console.ReadLine();
        }

        static void PrintResults(Dictionary<string, Dictionary<int, Dictionary<string, long>>> formattedValues)
        {
            using (var parentEnumerator = formattedValues.GetEnumerator())
            {
                while (parentEnumerator.MoveNext())
                {
                    Console.WriteLine();

                    Console.WriteLine();
                    Console.WriteLine(parentEnumerator.Current.Key);

                    Console.WriteLine("***************************************************************************************************************************************************************************************************");
                    bool isFirst = true;
                    using (var childEnumerator = parentEnumerator.Current.Value.GetEnumerator())
                    {
                        while (childEnumerator.MoveNext())
                        {
                            if (isFirst)
                            {
                                string header = string.Format("        {0}     |       {1}     |       {2}     |       {3}     |       {4}     |       {5}     |       {6}     ",
                                    "Iteration",
                                    Ado.PureAdoExecuter.ExecName,
                                    Dapper.DapperExecuter.ExecName,
                                    Dapper.DapperBufferedExecuter.ExecName,
                                    Dapper.DapperFirstOrDefaultExecuter.ExecName,
                                    Dapper.DapperContribExecuter.ExecName,
                                    EntityFramework.EntityFrameworNoTrackingExecuter.ExecName
                                    );
                                Console.WriteLine(header);
                                Console.WriteLine("----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------");
                                isFirst = false;
                            }

                            string row = string.Format("            {0}         |           {1}         |           {2}         |           {3}         |           {4}         |           {5}         |           {6}     ",
                                childEnumerator.Current.Key,
                                childEnumerator.Current.Value[Ado.PureAdoExecuter.ExecName],
                                childEnumerator.Current.Value[Dapper.DapperExecuter.ExecName],
                                childEnumerator.Current.Value[Dapper.DapperBufferedExecuter.ExecName],
                                childEnumerator.Current.Value[Dapper.DapperFirstOrDefaultExecuter.ExecName],
                                childEnumerator.Current.Value[Dapper.DapperContribExecuter.ExecName],
                                childEnumerator.Current.Value[EntityFramework.EntityFrameworNoTrackingExecuter.ExecName]);
                            Console.WriteLine(row);


                        }

                    }

                }

            }
        }

    }
}
