using System;
using TennisClub.Api.Models.Mongo;

namespace TennisClub.Api.Models.Output
{
    public class PlayerOutputModel
    {
        public string Id { get; set; }
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public DateTime BirthDate { get; set; }

        public Country Country { get; set; }

        public string Bio { get; set; }

        public string StrongHand { get; set; }

        public string RacketBrand { get; set; }

        public int Height { get; set; }

        public int Titles { get; set; }

        public int Weight { get; set; }

        public int Rank { get; set; }

        public string Picture { get; set; }

        public string StandingPicture { get; set; }

        public string Instagram { get; set; }

        public string Twitter { get; set; }

        public int TotalWins { get; set; }

        public int TotalLosses { get; set; }
    }
}
