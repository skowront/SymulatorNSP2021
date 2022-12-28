namespace SymulatorNSP.Server.Models
{
    public class MongoDBSettings
    {
        public string ConnectionString { get; set; } = null!;

        public string DatabaseName { get; set; } = null!;

        public string LeaderboardCollectionName { get; set; } = null!;
    }
}
