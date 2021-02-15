using System.Collections.Generic;
using TennisClub.Api.Models.Input;
using TennisClub.Api.Models.Mongo;

namespace TennisClub.Api.Models.Output
{
    public class TournamentOutputModel
    {
        public string Id { get; set; }
        public string Name { get; set; }

        public string City { get; set; }

        public Country Country { get; set; }

        public string Surface { get; set; }

        public string Category { get; set; }

        public string Logo { get; set; }

        public string AtpPoints { get; set; }

        public string TournamentInfo { get; set; }

        public ICollection<CourtInputOutputModel> Courts { get; set; }
    }
}
