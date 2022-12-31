using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SymulatorNSP.Core
{
    public class LeaderboardRecordContract
    {
        public LeaderboardRecord Record { get; set; } = new LeaderboardRecord();
        public string Key { get; set; } = string.Empty;
    }
}
