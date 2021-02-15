using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;

namespace TennisClub.Api.Models.Mongo
{
    public class Tournament : BaseMongoEntity
    {
        [BsonElement("name")]
        public string Name { get; set; }

        [BsonElement("city")]
        public string City { get; set; }

        [BsonElement("country")]
        public Country Country { get; set; }

        [BsonElement("surface")]
        public string Surface { get; set; }

        [BsonElement("category")]
        public string Category { get; set; }

        [BsonElement("atpPoints")]
        public string AtpPoints { get; set; }

        [BsonElement("logo")]
        public byte[] Logo { get; set; }

        [BsonElement("about")]
        public string TournamentInfo { get; set; }

        [BsonElement("courts")]
        public ICollection<Court> Courts { get; set; }

    }
}
