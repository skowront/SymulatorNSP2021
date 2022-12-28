using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SymulatorNSP.Core
{
    public class FittingResult
    {
        public string Original { get; set; } = string.Empty;
        public string Fit { get; set; } = string.Empty;
        public int Distance { get; set; } = 0;
        public string Algorithm { get; set; } = string.Empty;
        public bool Success = false;
    }
}
