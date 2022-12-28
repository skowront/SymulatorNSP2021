using MongoDB.Bson;
using SymulatorNSP.Client.Shared.Models;
using SymulatorNSP.Client.Shared.Services.Interfaces;
using SymulatorNSP.Core;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Net.Http.Json;
using static SymulatorNSP.Core.LeaderboardRecord;

namespace SymulatorNSP.Client.Shared.Services.Implementations
{
    public class LeaderboardService : ILeaderboardService
    {
        private readonly HttpClient httpClient;
        private readonly IConfigService configService;
        public PagingHelper PagingHelper { get; set; } = new PagingHelper(new PagingInfo());
        public int PagesCount
        {
            get { return this.PagingHelper.PageCount; }
        }
        public int CurrentPage { get; set; } = 0;

        public IList<LeaderboardRecord> Leaderboard { get; set; } = new List<LeaderboardRecord>();

        public LeaderboardService(HttpClient httpClient, IConfigService configService)
        {
            this.httpClient = httpClient;
            this.configService = configService;
        }

        public async Task Initialize()
        {
            var pagingInfo = await this.GetRecordsInfo();
            if (pagingInfo == null)
                return;
            this.CurrentPage = 0;
            this.PagingHelper = new PagingHelper(pagingInfo);
            this.Leaderboard = new List<LeaderboardRecord>((await this.GetRecords()) ?? new List<LeaderboardRecord>());
        }

        public void OnRecordAddedSuccess()
        {
            this.PagingHelper = new PagingHelper(new PagingInfo() { PageSize = 3, RecordsCount = this.Leaderboard.Count });
        }

        public async Task<eChangeResult> AddEntry(Tuple<LeaderboardRecord, string> recordKey)
        {
            var result = await this.httpClient.PostAsJsonAsync((await configService.GetRemoteAddresss()) + "/Leaderboard" , recordKey);
            if(result.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return await result.Content.ReadFromJsonAsync<eChangeResult>();
            }
            return eChangeResult.Unknown;
        }

        public async Task<IEnumerable<LeaderboardRecord>?> GetRecords()
        {
            return await this.httpClient.GetFromJsonAsync<IEnumerable<LeaderboardRecord>>((await configService.GetRemoteAddresss()) + "/Leaderboard");
        }

        public async Task<IEnumerable<LeaderboardRecord>?> GetRecords(int count)
        {
            return await this.httpClient.GetFromJsonAsync<IEnumerable<LeaderboardRecord>>((await configService.GetRemoteAddresss()) +  $"/Leaderboard/{count}");
        }

        public async Task<IEnumerable<LeaderboardRecord>?> GetRecords(int from, int to)
        {
            return await this.httpClient.GetFromJsonAsync<IEnumerable<LeaderboardRecord>>((await configService.GetRemoteAddresss()) + $"/Leaderboard/{from}/{to}");
        }

        public async Task<PagingInfo?> GetRecordsInfo()
        {
            return await this.httpClient.GetFromJsonAsync<PagingInfo>((await configService.GetRemoteAddresss()) + $"/Leaderboard/{nameof(GetRecordsInfo)}");
        }

        public async Task<double?> GetMeanFactor(LeaderboardRecord.eSource source)
        {
            return await this.httpClient.GetFromJsonAsync<double>((await configService.GetRemoteAddresss()) + $"/Leaderboard/{nameof(GetMeanFactor)}/{source}");
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
            var pageInfo = this.PagingHelper.GetPage(Math.Min(this.CurrentPage + 1, this.PagingHelper.PageCount - 1));
            this.CurrentPage = this.CurrentPage + 1;
            this.Leaderboard = new List<LeaderboardRecord>(await this.GetRecords(pageInfo.Begin, pageInfo.End) ?? new List<LeaderboardRecord>());
        }

        public async Task GoToPrevious()
        {
            var pageInfo = this.PagingHelper.GetPage(Math.Min(this.CurrentPage - 1, this.PagingHelper.PageCount - 1));
            this.CurrentPage = this.CurrentPage - 1;
            this.Leaderboard = new List<LeaderboardRecord>(await this.GetRecords(pageInfo.Begin, pageInfo.End) ?? new List<LeaderboardRecord>());
        }

        public async Task GoToPageNumber(int pageNumber)
        {
            var pageInfo = this.PagingHelper.GetPage(Math.Min(pageNumber, this.PagingHelper.PageCount - 1));
            this.CurrentPage = pageNumber;
            this.Leaderboard = new List<LeaderboardRecord>(await this.GetRecords(pageInfo.Begin, pageInfo.End) ?? new List<LeaderboardRecord>());
        }

        public int GetPageSize()
        {
            return this.PagingHelper.PagingInfo.PageSize;
        }
    }
}
