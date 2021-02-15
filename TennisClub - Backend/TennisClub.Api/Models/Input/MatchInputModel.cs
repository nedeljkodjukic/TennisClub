using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using TennisClub.Api.Models.Mongo;

namespace TennisClub.Api.Models.Input
{
    public class MatchInputModel
    {
        [Required]
        public string FirstPlayerId { get; set; }

        [Required]
        public string SecondPlayerId { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

        [Required]
        public string Phase { get; set; }

        [Required]
        public int Attendance { get; set; }

        [Required]
        public string CourtName { get; set; }

        [Required]
        public string TournamentId { get; set; }

        [Required]
        public ICollection<Set> Sets { get; set; }
    }
}
