using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace SymulatorNSP.Core
{
    public class LeaderboardRecord
    {
        public string Nickname { get; set; } = string.Empty;
        public int QueryCount { get; set; } = -1;
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.Now;
        public double Factor { get; set; } = 0;
        public int Threads { get; set; } = 1;
        public eSource Source { get; set; } = eSource.Unknown;
        public LeaderboardRecord() { }

        public TimeSpan ExecutionTime()
        {
            return this.EndTime - this.StartTime;
        }

        public enum eSource
        {
            Browser, Windows, Unknown, Any
        }

        public enum eChangeResult
        {
            Success, WrongKey, WorseFactor, Unknown, TooFewRecords, KeyEmpty
        }

        public const int MinAcceptedQueries = 1000000;
    }
}
