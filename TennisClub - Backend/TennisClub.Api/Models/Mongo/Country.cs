
using MongoDB.Bson.Serialization.Attributes;

namespace TennisClub.Api.Models.Mongo
{
    public class Country
    {
        [BsonElement("name")]
        public string Name { get; set; }

        [BsonElement("code")]
        public string Code { get; set; }
    }
}
