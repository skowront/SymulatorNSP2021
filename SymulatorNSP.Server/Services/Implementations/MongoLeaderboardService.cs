using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using SymulatorNSP.Client.Shared.Models;
using SymulatorNSP.Core;
using SymulatorNSP.Server.Models;
using SymulatorNSP.Server.Services.Interfaces;
using static SymulatorNSP.Core.LeaderboardRecord;

namespace SymulatorNSP.Server.Services.Implementations
{
    public class MongoLeaderboardService : ILeaderboardService
    {
        private const int RecordsLimitOnPage = 10;
        private readonly IConfiguration configuration;
        public IMongoCollection<MongoLeaderboardRecord> LeaderboardCollection { get; }

        public MongoLeaderboardService(IConfiguration configuration, IOptions<MongoDBSettings> settings) 
        {
            this.configuration = configuration;
            var mongoClient = new MongoClient(settings.Value.ConnectionString);

            var mongoDatabase = mongoClient.GetDatabase(settings.Value.DatabaseName);

            LeaderboardCollection =  mongoDatabase.GetCollection<MongoLeaderboardRecord>(settings.Value.LeaderboardCollectionName);
        }
         
        public async Task<LeaderboardRecord.eChangeResult> AddEntry(LeaderboardRecordContract contract)
        {
            var record = contract.Record;
            var Key = contract.Key;
            if (Key == null || Key == string.Empty)
                return eChangeResult.KeyEmpty;
            record.Timestamp = DateTime.Now;
            record.Factor = GUS.CalculateTimesFasterFactor(record.ExecutionTime(), record.QueryCount) ?? 0;
            var mongoRec = (await this.LeaderboardCollection.Find(rec => rec.Model.Nickname == record.Nickname).ToListAsync()).FirstOrDefault();
            if (mongoRec == null)
            {
                if (record.QueryCount < LeaderboardRecord.MinAcceptedQueries)
                {
                    return eChangeResult.TooFewRecords;
                }
                var mongoRecord = new MongoLeaderboardRecord(record) { Id = ObjectId.GenerateNewId(), Model = record, Key = Key };
                await this.LeaderboardCollection.InsertOneAsync(mongoRecord);
                return eChangeResult.Success;
            }
            else if(Key == string.Empty || Key == null)
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
                await this.LeaderboardCollection.ReplaceOneAsync(x => x.Id == mongoRec.Id, mongoRec);
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
            return (await this.LeaderboardCollection.Find(_ => true).SortByDescending(bson => bson.Model.Factor).Limit(RecordsLimitOnPage).ToListAsync()).Select((r) => { return r.Model; });
        }

        public async Task<IEnumerable<LeaderboardRecord>> GetRecords(int count)
        {
            return (await this.LeaderboardCollection.Find(_ => true).SortByDescending(bson => bson.Model.Factor).Limit(Math.Min(count,RecordsLimitOnPage)).ToListAsync()).Select((r) => { return r.Model; });
        }

        public async Task<IEnumerable<LeaderboardRecord>> GetRecords(int from, int to)
        {
            return (await this.LeaderboardCollection.Find(_ => true)
                .SortByDescending(bson => bson.Model.Factor)
                .Skip(from).Limit(Math.Min(Math.Abs(to-from), RecordsLimitOnPage))
                .ToListAsync())
                .Select((r) => { return r.Model; });
        }

        public async Task<PagingInfo> GetRecordsInfo()
        {
            return new PagingInfo() { RecordsCount = Convert.ToInt32(await this.LeaderboardCollection.CountDocumentsAsync(_ => true)), PageSize = RecordsLimitOnPage };
        }

        public async Task<double> GetMeanFactor(LeaderboardRecord.eSource source)
        {
            if(source == eSource.Any)
            {
                var list = await this.LeaderboardCollection.Aggregate().Match(_ => true).Group(b => true, g => new { avg = g.Average(p => p.Model.Factor) }).ToListAsync();
                return list.Select(_ => _.avg).FirstOrDefault(0);
            }
            else
            {
                var list = await this.LeaderboardCollection.Aggregate().Match(_ => _.Model.Source == source).Group(b => true, g => new { avg = g.Average(p => p.Model.Factor) }).ToListAsync();
                return list.Select(_ => _.avg).FirstOrDefault(0);
            }
        }
    }
}
