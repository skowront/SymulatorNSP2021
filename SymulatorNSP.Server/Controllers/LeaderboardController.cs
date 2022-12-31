using Microsoft.AspNetCore.Mvc;
using SymulatorNSP.Core;
using SymulatorNSP.Server.Services.Implementations;
using SymulatorNSP.Server.Services.Interfaces;

namespace SymulatorNSP.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LeaderboardController : Controller
    {
        private readonly ILeaderboardService leaderboardService;

        public LeaderboardController(ILeaderboardService leaderboardService)
        {
            this.leaderboardService = leaderboardService;
        }

        [HttpGet]
        public async Task<IActionResult> GetRecords()
        {
            return Ok(await this.leaderboardService.GetRecords());
        }

        [HttpGet("{" + nameof(count) + "}")]
        public async Task<IActionResult> GetRecords(int count)
        {
            return Ok(await this.leaderboardService.GetRecords(count));
        }

        [HttpGet("{" + nameof(from) + "}/{" + nameof(to) + "}")]
        public async Task<IActionResult> GetRecords(int from, int to)
        {
            return Ok(await this.leaderboardService.GetRecords(from, to));
        }

        [HttpGet(nameof(GetRecordsInfo))]
        public async Task<IActionResult> GetRecordsInfo()
        {
            return Ok(await this.leaderboardService.GetRecordsInfo());
        }

        [HttpGet(nameof(GetMeanFactor)+"/{"+nameof(source)+"}")]
        public async Task<IActionResult> GetMeanFactor(LeaderboardRecord.eSource source)
        {
            return Ok(await this.leaderboardService.GetMeanFactor(source));
        }

        [HttpPost]
        public async Task<IActionResult> AddRecord(LeaderboardRecordContract record)
        {
            return Ok(await this.leaderboardService.AddEntry(record));
        }
    }
}
