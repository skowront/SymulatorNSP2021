using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SymulatorNSP.Core
{
    public class Query
    {
        public string Result { get; set; } = String.Empty;
        public string Value { get; set; } = String.Empty;
        public int QueriedRecordsCount { get; set; } = 0;
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public TimeSpan ExecutionTime { get; set; }
    }
}
