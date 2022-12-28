using SymulatorNSP.Core.Fastenstein;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SymulatorNSP.Core
{
    public class LevensteinDistanceCalculator:DistanceCalculator
    {
        public LevensteinDistanceCalculator()
        {
            this.Name = "Levenstein";
        }

        public override int CalculateDistance(string s1, string s2)
        {
            return Levenshtein.Distance(s1, s2);
        }
    }
}
