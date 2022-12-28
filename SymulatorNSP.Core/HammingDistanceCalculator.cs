using SymulatorNSP.Core.HammingDistance;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;





namespace SymulatorNSP.Core
{
    public class HammingDistanceCalculator: DistanceCalculator
    {
        public HammingDistanceCalculator()
        {
            this.Name = "Hamming";
        }

        public override int CalculateDistance(string s1, string s2)
        {
            return HammingDistance.HammingDistance.CalculateHammingDistance(s1, s2);
        }
    }
}
