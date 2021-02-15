using System;

namespace TennisClub.Api.Models.Output
{
    public class TournamentWinnerOutputModel
    {
        public PlayerBasicOutputModel Player { get; set; }

        public int Year { get; set; }
    }
}
