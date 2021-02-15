
using MongoDB.Bson.Serialization.Attributes;

namespace TennisClub.Api.Models.Mongo
{
    public class SocialNetworkAccount
    {
        [BsonElement("socialNetwork")]
        public string SocialNetwork { get; set; }

        [BsonElement("link")]
        public string Link { get; set; }
    }
}
