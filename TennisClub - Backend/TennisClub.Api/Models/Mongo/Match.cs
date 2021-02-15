using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;

namespace TennisClub.Api.Models.Mongo
{
    public class Match : BaseMongoEntity
    {
        [BsonElement("first")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string FirstPlayerId { get; set; }

        [BsonElement("second")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string SecondPlayerId { get; set; }

        [BsonElement("date")]
        public DateTime Date { get; set; }

        //todo computed from sets duration
        [BsonElement("duration")]
        public int DurationInMinutes { get; set; } //in minutes

        //todo computer from sets
        [BsonElement("winner")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string WinnerId { get; set; }

        [BsonElement("round")]
        public string Phase { get; set; } //round

        [BsonElement("attendance")]
        public int Attendance { get; set; }

        [BsonElement("court")]
        public string CourtName { get; set; }

        [BsonElement("tournament")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string TournamentId { get; set; }

        [BsonElement("sets")]
        public ICollection<Set> Sets { get; set; }
    }
}
