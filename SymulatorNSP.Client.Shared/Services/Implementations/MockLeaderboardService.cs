using MongoDB.Bson;
using SymulatorNSP.Client.Shared.Models;
using SymulatorNSP.Client.Shared.Services.Interfaces;
using SymulatorNSP.Core;
using SymulatorNSP.Core.Resources;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Net.Http.Json;
using System.Reflection.Metadata.Ecma335;
using static SymulatorNSP.Core.LeaderboardRecord;

namespace SymulatorNSP.Client.Shared.Services.Implementations
{
    public class MockLeaderboardService : ILeaderboardService
    {
        public IList<LeaderboardRecord> Leaderboard { get; set; } = new List<LeaderboardRecord>();
        public Collection<LeaderboardRecord> Storage { get; set; } = new Collection<LeaderboardRecord>();
        public PagingHelper PagingHelper { get; set; } = null;
        public int PagesCount
        {
            get { return this.PagingHelper.PageCount; }
        }
        public int CurrentPage { get; set; }

        public MockLeaderboardService()
        {
            this.Storage.Add(new LeaderboardRecord() { Factor = 1});
            this.Storage.Add(new LeaderboardRecord() { Factor = 2});
            this.Storage.Add(new LeaderboardRecord() { Factor = 3});
            this.Storage.Add(new LeaderboardRecord() { Factor = 4});
            this.Storage.Add(new LeaderboardRecord() { Factor = 5});
            this.Storage.Add(new LeaderboardRecord() { Factor = 7});
            this.Storage.Add(new LeaderboardRecord() { Factor = 8});
            this.Storage.Add(new LeaderboardRecord() { Factor = 9});
            this.Storage.Add(new LeaderboardRecord() { Factor = 11});
            this.Storage.Add(new LeaderboardRecord() { Factor = 0.5});
            this.Storage.Add(new LeaderboardRecord() { Factor = 10});
            this.Storage.Add(new LeaderboardRecord() { Factor = 12});
            this.Storage.Add(new LeaderboardRecord() { Factor = 113});
            this.PagingHelper = new PagingHelper(new PagingInfo() { PageSize = 10, RecordsCount = this.Storage.Count });
        }

        public void OnRecordAddedSuccess()
        {
            this.PagingHelper = new PagingHelper(new PagingInfo() { PageSize = 10, RecordsCount = this.Storage.Count });
        }

        public async Task<eChangeResult> AddEntry(LeaderboardRecordContract contract)
        {
            try
            {
                var record = contract.Record;
                var Key = contract.Key;
                if (Key == null || Key == string.Empty)
                    return eChangeResult.KeyEmpty;
                record.Factor = GUS.CalculateTimesFasterFactor(record.ExecutionTime(), record.QueryCount) ?? 0;
                var mongoRec = this.Storage.Where((rec) => { return rec.Nickname == record.Nickname; }).FirstOrDefault();
                if (mongoRec == null)
                {
                    if (record.QueryCount < LeaderboardRecord.MinAcceptedQueries)
                    {
                        return eChangeResult.TooFewRecords;
                    }
                    this.Storage.Add(record);
                    this.PagingHelper = new PagingHelper(new PagingInfo() { PageSize = 10, RecordsCount = this.Storage.Count });
                    return eChangeResult.Success;
                }
                else if (Key == string.Empty || Key == null)
                {
                    return eChangeResult.KeyEmpty;
                }
                else
                {
                    if (record.QueryCount < LeaderboardRecord.MinAcceptedQueries)
                    {
                        return eChangeResult.TooFewRecords;
                    }
                    else if (record.Factor > mongoRec.Factor)
                    {
                        return eChangeResult.WorseFactor;
                    }
                    mongoRec = record;
                    this.PagingHelper = new PagingHelper(new PagingInfo() { PageSize = 10, RecordsCount = this.Storage.Count });
                    return eChangeResult.Success;
                }
            }
            catch (Exception)
            {
                return eChangeResult.Unknown;
            }
        }

        public async Task<IEnumerable<LeaderboardRecord>?> GetRecords()
        {
            var list = Storage.ToList();
            list.Sort(CompareRecordsByExecutionTime);
            return list.Take(this.PagingHelper.PagingInfo.PageSize).Select((r) => { return r; });
        }

        public async Task<IEnumerable<LeaderboardRecord>?> GetRecords(int count)
        {
            var list = Storage.ToList();
            list.Sort(CompareRecordsByExecutionTime);
            return list.Take(Math.Min(count, this.PagingHelper.PagingInfo.PageSize)).Select((r) => { return r; });
        }

        public async Task<IEnumerable<LeaderboardRecord>?> GetRecords(int from, int to)
        {
            var list = Storage.ToList();
            list.Sort(CompareRecordsByExecutionTime);
            return list.Take(new Range(from, from + Math.Min( Math.Abs(to - from), this.PagingHelper.PagingInfo.PageSize))).Select((r) => { return r; });
        }

        private static int CompareRecordsByExecutionTime(LeaderboardRecord p1, LeaderboardRecord p2)
        {
            if (p1.Factor < p2.Factor)
            {
                return 1;
            }
            if (p1.Factor > p2.Factor)
            {
                return -1;
            }
            return 0;
        }

        public async Task<double?> GetMeanFactor(LeaderboardRecord.eSource source)
        {
            if (source == eSource.Any)
            {
                return Storage.Average((s) => { return s.Factor; });
            }
            else
            {
                return Storage.Where((p) => { return p.Source == source; }).Average((s) => { return s.Factor; });
            }
        }

        public async Task<PagingInfo?> GetRecordsInfo()
        {
            return this.PagingHelper.PagingInfo;
        }

        public async Task Initialize()
        {
            var pagingInfo = await this.GetRecordsInfo();
            if (pagingInfo == null)
                return;
            this.CurrentPage = 0;
            //this.PagingHelper  = new PagingHelper(pagingInfo);
            //this.Leaderboard = new List<LeaderboardRecord>((await this.GetRecords()) ?? new List<LeaderboardRecord>());
        }

        public async Task GoToFirst()
        {
            var pageInfo = this.PagingHelper.GetPage(0);
            this.CurrentPage = 0;
            this.Leaderboard = new List<LeaderboardRecord>(await this.GetRecords(pageInfo.Begin, pageInfo.End) ?? new List<LeaderboardRecord>());
        }

        public async Task GoToLast()
        {
            var pageInfo = this.PagingHelper.GetPage(this.PagingHelper.PageCount - 1);
            this.CurrentPage = this.PagingHelper.PageCount - 1;
            this.Leaderboard = new List<LeaderboardRecord>(await this.GetRecords(pageInfo.Begin, pageInfo.End) ?? new List<LeaderboardRecord>());
        }

        public async Task GoToNext()
        {
            var pageInfo = this.PagingHelper?.GetPage(Math.Min(this.CurrentPage + 1, this.PagingHelper.PageCount - 1));
            this.CurrentPage = this.CurrentPage + 1;
            this.Leaderboard = new List<LeaderboardRecord>(await this.GetRecords(pageInfo.Begin, pageInfo.End) ?? new List<LeaderboardRecord>());
        }

        public async Task GoToPrevious()
        {
            var pageInfo = this.PagingHelper?.GetPage(Math.Min(this.CurrentPage - 1, this.PagingHelper.PageCount - 1));
            this.CurrentPage = this.CurrentPage - 1;
            this.Leaderboard = new List<LeaderboardRecord>(await this.GetRecords(pageInfo.Begin, pageInfo.End) ?? new List<LeaderboardRecord>());
        }

        public async Task GoToPageNumber(int pageNumber)
        {
            var pageInfo = this.PagingHelper?.GetPage(Math.Min(pageNumber, this.PagingHelper.PageCount - 1));
            this.CurrentPage = pageNumber;
            this.Leaderboard = new List<LeaderboardRecord>(await this.GetRecords(pageInfo.Begin, pageInfo.End) ?? new List<LeaderboardRecord>());
        }

        public int GetPageSize()
        {
            return this.PagingHelper.PagingInfo.PageSize;
        }
    }
}
