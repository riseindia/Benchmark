
using Benchmark.EF;
using Core;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Benchmark.EF
{
    public class EFNoTrackingExecuter : IExecuter
    {
        TestContext ctx;

        public static string ExecName
        {
            get
            {
                return "Entity Framework";
            }
        }
        public string Name
        {
            get
            {
                return "Entity Framework";
            }
        }

        public void Init(string connectionString) => ctx = new TestContext(connectionString);

        public ITest GetItemAsObject(int Id)
        {
            return ctx.Tests.AsNoTracking().Where(x => x.Id == Id) as ITest;

        }


        public IList<ITest> GetAllItemsAsObject()
        {
            return ctx.Tests.AsNoTracking().ToList<ITest>();
        }

        public void Finish()
        {

        }

        public void WriteOnce(int recordCount)
        {

            var tests = new List<Test>();
            Stopwatch s = new Stopwatch();
            s.Start();
            for (int i = 0; i < recordCount; i++)
            {
                tests.Add(new Test { Text = "erere", CreationDate = DateTime.Now, LastChangeDate = DateTime.Now });
            }
            s.Stop();
            Console.WriteLine("Time taken in constructing object from " + Name + "  is : " + s.ElapsedMilliseconds);

            ctx.Tests.AddRange(tests);
            ctx.SaveChanges();
        }

        public void WriteInLoop(int recordCount)
        {

            var tests = new List<Test>();
            Stopwatch s = new Stopwatch();
            s.Start();
            for (int i = 0; i < recordCount; i++)
            {
                var test = new Test { Text = "erere", CreationDate = DateTime.Now, LastChangeDate = DateTime.Now };
                ctx.Tests.Add(test);
            }
            ctx.SaveChanges();
        }
    }
}
