using MongoDB.Bson.Serialization.Attributes;

namespace TennisClub.Api.Models.Mongo
{
    public class Court
    {
        [BsonElement("name")]
        public string Name { get; set; }

        [BsonElement("picture")]
        public byte[] Picture { get; set; }

        [BsonElement("capacity")]
        public int Capacity { get; set; }
    }
}
