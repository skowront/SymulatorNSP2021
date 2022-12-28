using MongoDB.Bson;
using MongoDB.Driver.Core.Operations;
using SymulatorNSP.Client.Shared.Models;
using SymulatorNSP.Core;
using SymulatorNSP.Server.Models;
using SymulatorNSP.Server.Services.Interfaces;
using System.Collections.ObjectModel;
using static SymulatorNSP.Core.LeaderboardRecord;

namespace SymulatorNSP.Server.Services.Implementations
{
    public class LeaderboardService : ILeaderboardService
    {
        private const int RecordsLimitOnPage = int.MaxValue;
        public Collection<MongoLeaderboardRecord> Storage { get; set; } = new Collection<MongoLeaderboardRecord>();

        public LeaderboardService() 
        {
        }

        public async Task<eChangeResult> AddEntry(Tuple<LeaderboardRecord, string> recordKey)
        {
            var record = recordKey.Item1;
            var Key = recordKey.Item2;
            if (Key == null || Key == string.Empty)
                return eChangeResult.KeyEmpty;
            record.Timestamp = DateTime.Now;
            record.Factor = GUS.CalculateTimesFasterFactor(record.ExecutionTime(), record.QueryCount) ?? 0;
            var mongoRec = this.Storage.Where((rec) => { return rec.Model.Nickname == record.Nickname; }).FirstOrDefault();
            if (mongoRec == null)
            {
                if (record.QueryCount < LeaderboardRecord.MinAcceptedQueries)
                {
                    return eChangeResult.TooFewRecords;
                }
                var mongoRecord = new MongoLeaderboardRecord(record) { Id = ObjectId.GenerateNewId(), Model = record };
                this.Storage.Add(mongoRecord);
                return eChangeResult.Success;
            }
            else if (Key == string.Empty || Key == null)
            {
                return eChangeResult.KeyEmpty;
            }
            else if (mongoRec.Key == Key)
            {
                if (record.QueryCount < LeaderboardRecord.MinAcceptedQueries)
                {
                    return eChangeResult.TooFewRecords;
                }
                else if (record.Factor > mongoRec.Model.Factor)
                {
                    return eChangeResult.WorseFactor;
                }
                mongoRec.Model = record;
                return eChangeResult.Success;
            }
            else if (mongoRec.Key != Key)
            {
                return eChangeResult.WrongKey;
            }
            return eChangeResult.Unknown;
        }

        public async Task<IEnumerable<LeaderboardRecord>> GetRecords()
        {
            var list = Storage.ToList();
            list.Sort(CompareRecordsByExecutionTime);
            return list.Select((r) => { return r.Model; });
        }

        public async Task<IEnumerable<LeaderboardRecord>> GetRecords(int count)
        {
            var list = Storage.ToList();
            list.Sort(CompareRecordsByExecutionTime);
            return list.Take(count).Select((r) => { return r.Model; });
        }

        public async Task<IEnumerable<LeaderboardRecord>> GetRecords(int from, int to)
        {
            var list = Storage.ToList();
            list.Sort(CompareRecordsByExecutionTime);
            return list.Take(new Range(from,to)).Select((r) => { return r.Model; });
        }
        public async Task<double> GetMeanFactor(LeaderboardRecord.eSource source)
        {
            if(source == eSource.Any)
            {
                return Storage.Average((s) => { return s.Model.Factor; });
            }
            else
            {
                return Storage.Where((p) => { return p.Model.Source == source; }).Average((s) => { return s.Model.Factor; });
            }
        }

        public async Task<PagingInfo> GetRecordsInfo()
        {
            return new PagingInfo() { RecordsCount = this.Storage.Count, PageSize = RecordsLimitOnPage };
        }

        private static int CompareRecordsByExecutionTime(MongoLeaderboardRecord p1, MongoLeaderboardRecord p2)
        {
            if (p1.Model.Factor > p2.Model.Factor)
            {
                return 1;
            }
            if (p1.Model.Factor < p2.Model.Factor)
            {
                return -1;
            }
            return 0;
        }

    }
}
