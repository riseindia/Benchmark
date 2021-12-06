using System;
using System.Collections.Generic;
using System.Text;

namespace Core
{
    public interface ITest
    {
        int Id { get; set; }
        string Text { get; set; }
        DateTime CreationDate { get; set; }
        DateTime LastChangeDate { get; set; }
    }
}
