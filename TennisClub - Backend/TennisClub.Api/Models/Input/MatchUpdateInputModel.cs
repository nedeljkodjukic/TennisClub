using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using TennisClub.Api.Models.Mongo;

namespace TennisClub.Api.Models.Input
{
    public class MatchUpdateInputModel
    {
        [Required]
        public DateTime Date { get; set; }

        [Required]
        public string Phase { get; set; }

        [Required]
        public int Attendance { get; set; }

        [Required]
        public string CourtName { get; set; }

        [Required]
        public ICollection<Set> Sets { get; set; }
    }
}
