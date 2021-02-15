using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;


namespace TennisClub.Api.Models.Mongo
{
    public class Player : BaseMongoEntity
    {
        [BsonElement("name")]
        public string FirstName { get; set; }

        [BsonElement("surname")]
        public string LastName { get; set; }

        [BsonElement("birthDate")]
        [BsonDateTimeOptions(DateOnly = true)]
        public DateTime BirthDate { get; set; }

        [BsonElement("country")]
        public Country Country { get; set; }

        [BsonElement("bio")]
        public string Bio { get; set; }

        [BsonElement("hand")]
        public string StrongHand { get; set; }

        [BsonElement("racket")]
        public string RacketBrand { get; set; }

        [BsonElement("height")]
        public int Height { get; set; }

        [BsonElement("title")]
        public int Titles { get; set; }

        [BsonElement("weight")]
        public int Weight { get; set; }

        [BsonElement("networks")]
        public ICollection<SocialNetworkAccount> Accounts { get; set; }

        [BsonElement("picture")]
        public byte[] Picture { get; set; }

        [BsonElement("standingPicture")]
        public byte[] StandingPicture { get; set; }

        /*
        [BsonElement("matches")]
        [BsonRepresentation(BsonType.ObjectId)]
        public ICollection<string> Matches { get; set; }
        */

    }
}
