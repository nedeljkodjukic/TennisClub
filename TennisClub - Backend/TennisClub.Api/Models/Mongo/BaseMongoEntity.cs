using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace TennisClub.Api.Models.Mongo
{
    public class BaseMongoEntity
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
    }
}
