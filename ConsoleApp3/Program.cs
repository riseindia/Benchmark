using Core;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Versioning;

namespace ConsoleApp3
{
    class Program
    {
        static void Main(string[] args)
        {
            string connStr = @"Data Source =(local); Initial Catalog = OrmBenchmark; Integrated Security = True";

            // Set up data directory
            string runDir = System.AppContext.BaseDirectory;
            //string runDir = AppDomain.CurrentDomain.BaseDirectory;
            connStr = connStr.Replace("|DataDirectory|", runDir.TrimEnd('\\'));
            bool reRun = true;
            while (reRun)
            {
                Dictionary<int, Benchmarker> resultDic = new Dictionary<int, Benchmarker>();
                List<int> testCases = new List<int>
            {
                1,
                10,
                100,
                //1000,
                //10000, 
                //100000 
            };
                var ver = Assembly.GetEntryAssembly()?.GetCustomAttribute<TargetFrameworkAttribute>()?.FrameworkName;
                Console.WriteLine(ver);
                Console.WriteLine("Connection string: {0}", connStr);
                for (int i = 0; i < testCases.Count; i++)
                {
                    int recordcount = testCases[i];
                    var benchmarker = new Benchmarker(connStr, recordcount);

                    benchmarker.RegisterOrmExecuter(new Benchmark.ADONet.PureAdoExecuter());
                    benchmarker.RegisterOrmExecuter(new Benchmark.Dapper.DapperExecuter());
                    benchmarker.RegisterOrmExecuter(new Benchmark.Dapper.DapperContribExecuter());
                    benchmarker.RegisterOrmExecuter(new Benchmark.EF.EFNoTrackingExecuter());

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

                    resVal.Add(Benchmark.ADONet.PureAdoExecuter.ExecName, resultDic[keyEnumerator.Current].singleWriteResults.Where(x => x.Name == Benchmark.ADONet.PureAdoExecuter.ExecName).Select(x => x.ExecTime).FirstOrDefault());
                    resVal.Add(Benchmark.Dapper.DapperExecuter.ExecName, resultDic[keyEnumerator.Current].singleWriteResults.Where(x => x.Name == Benchmark.Dapper.DapperExecuter.ExecName).Select(x => x.ExecTime).FirstOrDefault());
                    //resVal.Add(Benchmark.Dapper.DapperBufferedExecuter.ExecName, resultDic[keyEnumerator.Current].singleWriteResults.Where(x => x.Name == Benchmark.Dapper.DapperBufferedExecuter.ExecName).Select(x => x.ExecTime).FirstOrDefault());
                    //resVal.Add(Benchmark.Dapper.DapperFirstOrDefaultExecuter.ExecName, resultDic[keyEnumerator.Current].singleWriteResults.Where(x => x.Name == Benchmark.Dapper.DapperFirstOrDefaultExecuter.ExecName).Select(x => x.ExecTime).FirstOrDefault());
                    resVal.Add(Benchmark.Dapper.DapperContribExecuter.ExecName, resultDic[keyEnumerator.Current].singleWriteResults.Where(x => x.Name == Benchmark.Dapper.DapperContribExecuter.ExecName).Select(x => x.ExecTime).FirstOrDefault());
                    resVal.Add(Benchmark.EF.EFNoTrackingExecuter.ExecName, resultDic[keyEnumerator.Current].singleWriteResults.Where(x => x.Name == Benchmark.EF.EFNoTrackingExecuter.ExecName).Select(x => x.ExecTime).FirstOrDefault());


                    formattedValues["Single Write"].Add(keyEnumerator.Current, resVal);

                    var resVal1 = new Dictionary<string, long>();

                    resVal1.Add(Benchmark.ADONet.PureAdoExecuter.ExecName, resultDic[keyEnumerator.Current].combinedWriteResults.Where(x => x.Name == Benchmark.ADONet.PureAdoExecuter.ExecName).Select(x => x.ExecTime).FirstOrDefault());
                    resVal1.Add(Benchmark.Dapper.DapperExecuter.ExecName, resultDic[keyEnumerator.Current].combinedWriteResults.Where(x => x.Name == Benchmark.Dapper.DapperExecuter.ExecName).Select(x => x.ExecTime).FirstOrDefault());
                    //resVal1.Add(Benchmark.Dapper.DapperBufferedExecuter.ExecName, resultDic[keyEnumerator.Current].combinedWriteResults.Where(x => x.Name == Benchmark.Dapper.DapperBufferedExecuter.ExecName).Select(x => x.ExecTime).FirstOrDefault());
                    //resVal1.Add(Benchmark.Dapper.DapperFirstOrDefaultExecuter.ExecName, resultDic[keyEnumerator.Current].combinedWriteResults.Where(x => x.Name == Benchmark.Dapper.DapperFirstOrDefaultExecuter.ExecName).Select(x => x.ExecTime).FirstOrDefault());
                    resVal1.Add(Benchmark.Dapper.DapperContribExecuter.ExecName, resultDic[keyEnumerator.Current].combinedWriteResults.Where(x => x.Name == Benchmark.Dapper.DapperContribExecuter.ExecName).Select(x => x.ExecTime).FirstOrDefault());
                    resVal1.Add(Benchmark.EF.EFNoTrackingExecuter.ExecName, resultDic[keyEnumerator.Current].combinedWriteResults.Where(x => x.Name == Benchmark.EF.EFNoTrackingExecuter.ExecName).Select(x => x.ExecTime).FirstOrDefault());


                    formattedValues["Combined Write"].Add(keyEnumerator.Current, resVal1);

                    var resVal2 = new Dictionary<string, long>();
                    resVal2.Add(Benchmark.ADONet.PureAdoExecuter.ExecName, resultDic[keyEnumerator.Current].singleReadresults.Where(x => x.Name == Benchmark.ADONet.PureAdoExecuter.ExecName).Select(x => x.ExecTime).FirstOrDefault());
                    resVal2.Add(Benchmark.Dapper.DapperExecuter.ExecName, resultDic[keyEnumerator.Current].singleReadresults.Where(x => x.Name == Benchmark.Dapper.DapperExecuter.ExecName).Select(x => x.ExecTime).FirstOrDefault());
                    //resVal2.Add(Benchmark.Dapper.DapperBufferedExecuter.ExecName, resultDic[keyEnumerator.Current].singleReadresults.Where(x => x.Name == Benchmark.Dapper.DapperBufferedExecuter.ExecName).Select(x => x.ExecTime).FirstOrDefault());
                    //resVal2.Add(Benchmark.Dapper.DapperFirstOrDefaultExecuter.ExecName, resultDic[keyEnumerator.Current].singleReadresults.Where(x => x.Name == Benchmark.Dapper.DapperFirstOrDefaultExecuter.ExecName).Select(x => x.ExecTime).FirstOrDefault());
                    resVal2.Add(Benchmark.Dapper.DapperContribExecuter.ExecName, resultDic[keyEnumerator.Current].singleReadresults.Where(x => x.Name == Benchmark.Dapper.DapperContribExecuter.ExecName).Select(x => x.ExecTime).FirstOrDefault());
                    resVal2.Add(Benchmark.EF.EFNoTrackingExecuter.ExecName, resultDic[keyEnumerator.Current].singleReadresults.Where(x => x.Name == Benchmark.EF.EFNoTrackingExecuter.ExecName).Select(x => x.ExecTime).FirstOrDefault());

                    formattedValues["Single Read"].Add(keyEnumerator.Current, resVal2);

                    var resVal3 = new Dictionary<string, long>();
                    resVal3.Add(Benchmark.ADONet.PureAdoExecuter.ExecName, resultDic[keyEnumerator.Current].combinedReadresultsForAllItems.Where(x => x.Name == Benchmark.ADONet.PureAdoExecuter.ExecName).Select(x => x.ExecTime).FirstOrDefault());
                    resVal3.Add(Benchmark.Dapper.DapperExecuter.ExecName, resultDic[keyEnumerator.Current].combinedReadresultsForAllItems.Where(x => x.Name == Benchmark.Dapper.DapperExecuter.ExecName).Select(x => x.ExecTime).FirstOrDefault());
                    //resVal3.Add(Benchmark.Dapper.DapperBufferedExecuter.ExecName, resultDic[keyEnumerator.Current].combinedReadresultsForAllItems.Where(x => x.Name == Benchmark.Dapper.DapperBufferedExecuter.ExecName).Select(x => x.ExecTime).FirstOrDefault());
                    //resVal3.Add(Benchmark.Dapper.DapperFirstOrDefaultExecuter.ExecName, resultDic[keyEnumerator.Current].combinedReadresultsForAllItems.Where(x => x.Name == Benchmark.Dapper.DapperFirstOrDefaultExecuter.ExecName).Select(x => x.ExecTime).FirstOrDefault());
                    resVal3.Add(Benchmark.Dapper.DapperContribExecuter.ExecName, resultDic[keyEnumerator.Current].combinedReadresultsForAllItems.Where(x => x.Name == Benchmark.Dapper.DapperContribExecuter.ExecName).Select(x => x.ExecTime).FirstOrDefault());
                    resVal3.Add(Benchmark.EF.EFNoTrackingExecuter.ExecName, resultDic[keyEnumerator.Current].combinedReadresultsForAllItems.Where(x => x.Name == Benchmark.EF.EFNoTrackingExecuter.ExecName).Select(x => x.ExecTime).FirstOrDefault());
                    formattedValues["Combined Read"].Add(keyEnumerator.Current, resVal3);
                    keyEnumerator.MoveNext();
                }
                PrintResults(formattedValues);
                Console.WriteLine("Finished.");

                Console.WriteLine("Do you want to run again?  (Y | N) ");
                var input = Console.ReadLine();
                if (input == "Y")
                    reRun = true;
                else
                {
                    if (input == "N")
                        reRun = false;
                    else
                        Console.WriteLine("Invalid input. Only  (Y | N) ");

                }
            }
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
                                string header = string.Format("        {0}     |       {1}     |       {2}     |       {3}     |       {4}     ",
                                    "Iteration",
                                    Benchmark.ADONet.PureAdoExecuter.ExecName,
                                    Benchmark.Dapper.DapperExecuter.ExecName,
                                    Benchmark.Dapper.DapperContribExecuter.ExecName,
                                    Benchmark.EF.EFNoTrackingExecuter.ExecName
                                    );
                                Console.WriteLine(header);
                                Console.WriteLine("----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------");
                                isFirst = false;
                            }

                            string row = string.Format("            {0}         |           {1}         |           {2}         |           {3}         |           {4}         ",
                                childEnumerator.Current.Key,
                                childEnumerator.Current.Value[Benchmark.ADONet.PureAdoExecuter.ExecName],
                                childEnumerator.Current.Value[Benchmark.Dapper.DapperExecuter.ExecName],
                                childEnumerator.Current.Value[Benchmark.Dapper.DapperContribExecuter.ExecName],
                                childEnumerator.Current.Value [Benchmark.EF.EFNoTrackingExecuter.ExecName]);
                            Console.WriteLine(row);


                        }

                    }

                }

            }
        }

    }
}
