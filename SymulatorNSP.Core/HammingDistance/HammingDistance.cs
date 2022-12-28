using System.Linq;


namespace SymulatorNSP.Core.HammingDistance
{
    public static partial class HammingDistance
    {
        public static int CalculateHammingDistance(this string source, string target)
        {
            if (source.Length != target.Length) return int.MaxValue;

            return source.Where((t, i) => !t.Equals(target[i])).Count();
        }
    }
}