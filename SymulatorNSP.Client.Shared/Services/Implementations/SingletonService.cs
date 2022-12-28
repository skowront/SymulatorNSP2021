using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SymulatorNSP.Client.Shared.Services.Implementations
{
    public class SingletonService
    {
        private int furthestStepsCompletedOnGUSOnline { get; set; } = 0;
        public int FurthestStepsCompletedOnGUSOnline
        {
            get { return furthestStepsCompletedOnGUSOnline; }
            set
            {
                furthestStepsCompletedOnGUSOnline = value < furthestStepsCompletedOnGUSOnline ? furthestStepsCompletedOnGUSOnline : value;
            }
        }
        public int LastSelectedStep { get; set; } = 0;
    }
}
