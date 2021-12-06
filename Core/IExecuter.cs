using System;
using System.Collections.Generic;
using System.Text;

namespace Core
{
    public interface IExecuter
    {
        void WriteOnce(int recordCount);
        void WriteInLoop(int recordCount);

        string Name { get; }
        void Init(string connectionStrong);
        ITest GetItemAsObject(int Id);
        IList<ITest> GetAllItemsAsObject();
        void Finish();
    }
}
