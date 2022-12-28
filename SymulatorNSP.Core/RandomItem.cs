using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SymulatorNSP.Core
{
    public class RandomItem<T>
    {
        public T Value { get; set; }
        public double Probability { get; set; }

        public RandomItem(T value, double probability)
        {
            Value = value;
            Probability = probability;
        }
    }
}
