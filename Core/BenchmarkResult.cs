using System;
using System.Collections.Generic;
using System.Text;

namespace Core
{
    public class BenchmarkResult
    {
        public string Name { get; set; }
        public long FirstItemExecTime { get; set; }
        public long ExecTime { get; set; }
    }
}
