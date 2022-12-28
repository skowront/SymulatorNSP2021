using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SymulatorNSP.Core
{
    public class DistanceCalculator
    {
        public string Name { get; set; } = "-"; //Domyślny == Default
        public virtual int CalculateDistance(string s1, string s2)
        {
            return int.MaxValue;
        }
    }
}
