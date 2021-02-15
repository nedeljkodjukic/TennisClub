using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using TennisClub.Api.Models.Mongo;

namespace TennisClub.Api.Models.Input
{
    public class TournamentInputModel
    {
        [Required]
        public string Name { get; set; }


        [Required]
        public string City { get; set; }


        [Required]
        public Country Country { get; set; }


        [Required]
        public string Surface { get; set; }


        [Required]
        public string Category { get; set; }


        [Required]
        public string AtpPoints { get; set; }


        [Required]
        public string Logo { get; set; }


        [Required]
        public string TournamentInfo { get; set; }


        [Required]
        public IEnumerable<CourtInputOutputModel> Courts { get; set; }
    }
}
