using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using SymulatorNSP.Core;

namespace SymulatorNSP.Server.Models
{
    public class MongoLeaderboardRecord
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public ObjectId Id { get; set; }
        public string Key { get; set; } = string.Empty;
        public LeaderboardRecord Model { get;set; }
        public MongoLeaderboardRecord() 
        {
        }
        public MongoLeaderboardRecord(LeaderboardRecord model) 
        {
            this.Model = model;
        }
    }
}
