using MongoDB.Bson;
using SymulatorNSP.Client.Shared.Models;
using SymulatorNSP.Core;
using SymulatorNSP.Server.Models;
using static SymulatorNSP.Core.LeaderboardRecord;

namespace SymulatorNSP.Server.Services.Interfaces
{
    public interface ILeaderboardService
    {
        public Task<eChangeResult> AddEntry(LeaderboardRecordContract record);
        public Task<IEnumerable<LeaderboardRecord>> GetRecords();
        public Task<IEnumerable<LeaderboardRecord>> GetRecords(int count);
        public Task<IEnumerable<LeaderboardRecord>> GetRecords(int from, int to);
        public Task<double> GetMeanFactor(LeaderboardRecord.eSource source);
        public Task<PagingInfo> GetRecordsInfo();
    }
}
