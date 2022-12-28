using SymulatorNSP.Core;

namespace SymulatorNSP.ViewModels
{
    public class SimpleGUSViewModel: GUSViewModel
    {
        public SimpleGUSViewModel(int virtualizedCount) : base()
        {
            this.DesiredPopulationCount = GUS.DefaultCacheSize;
            this.VirtualCount = virtualizedCount;
        }
    }
}
