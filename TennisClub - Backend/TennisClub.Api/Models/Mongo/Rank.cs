
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace TennisClub.Api.Models.Mongo
{
    public class Rank : BaseMongoEntity
    {
        [BsonElement("year")]
        public int Year { get; set; }

        [BsonElement("week")]
        public int Week { get; set; }

        [BsonElement("weekAsString")]
        public string WeekDuration { get; set; }

        [BsonElement("player")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string PlayerId { get; set; }

        [BsonElement("ranking")]
        public int RankNumber { get; set; }

        [BsonElement("previousRanking")]
        public int PreviousRankNumber { get; set; }

        [BsonElement("points")]
        public int Points { get; set; }
    }
}
