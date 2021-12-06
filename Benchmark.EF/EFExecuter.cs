using Benchmark.EF;
using Core;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Benchmark.EF
{
    public class EFExecuter : IExecuter
    {
        TestContext ctx;

        public string Name
        {
            get
            {
                return "Entity Framework";
            }
        }

        public void Init(string connectionStrong)
        {

            ctx = new TestContext(connectionStrong);

        }

        public ITest GetItemAsObject(int Id)
        {
            return ctx.Tests.Where(p => p.Id == Id) as ITest;

        }

        public dynamic GetItemAsDynamic(int Id)
        {
            return ctx.Tests.Where(p => p.Id == Id).Select(p => new {
                p.Id,
                p.Text,
                p.CreationDate,
                p.LastChangeDate
            });
        }

        public IList<ITest> GetAllItemsAsObject()
        {
            return ctx.Tests.ToList<ITest>();
        }

        public IList<dynamic> GetAllItemsAsDynamic()
        {
            return ctx.Tests.Select(p => new {
                p.Id,
                p.Text,
                p.CreationDate,
                p.LastChangeDate
            }).ToList<dynamic>();
        }
        public void Finish()
        {

        }

        public void WriteOnce(int recordCount)
        {
            throw new NotImplementedException();
        }

        public void WriteInLoop(int recordCount)
        {
            throw new NotImplementedException();
        }
    }
}
