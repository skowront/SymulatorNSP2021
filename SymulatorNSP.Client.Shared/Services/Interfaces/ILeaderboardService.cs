using SymulatorNSP.Client.Shared.Models;
using SymulatorNSP.Core;
using static SymulatorNSP.Core.LeaderboardRecord;

namespace SymulatorNSP.Client.Shared.Services.Interfaces
{
    public interface ILeaderboardService: IPageNavigable
    {
        IList<LeaderboardRecord> Leaderboard { get; set; }
        public Task<eChangeResult> AddEntry(LeaderboardRecordContract record);
        public Task<IEnumerable<LeaderboardRecord>?> GetRecords();
        public Task<IEnumerable<LeaderboardRecord>?> GetRecords(int count);
        public Task<IEnumerable<LeaderboardRecord>?> GetRecords(int from, int to);
        public Task<PagingInfo?> GetRecordsInfo();
        public Task<double?> GetMeanFactor(LeaderboardRecord.eSource source);
        public Task Initialize();
        public void OnRecordAddedSuccess();
    }
}
