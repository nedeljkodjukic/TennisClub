
using MongoDB.Bson.Serialization.Attributes;

namespace TennisClub.Api.Models.Mongo
{
    public class Set
    {
        [BsonElement("duration")]
        public int Duration { get; set; }//in minutes

        [BsonElement("firstScore")]
        public int FirstPlayerScore { get; set; }

        [BsonElement("secondScore")]
        public int SecondPlayerScore { get; set; }
    }
}
