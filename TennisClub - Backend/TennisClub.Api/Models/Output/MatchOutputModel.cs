using System;
using System.Collections.Generic;
using TennisClub.Api.Models.Mongo;

namespace TennisClub.Api.Models.Output
{
    public class MatchOutputModel
    {
        public string FirstPlayerFullName { get; set; }

        public string SecondPlayerFullName { get; set; }

        public DateTime Date { get; set; }

        public int DurationInMinutes { get; set; }

        public string Phase { get; set; }

        public int Attendance { get; set; }

        public string CourtName { get; set; }

        public string TournamentName { get; set; }

        public ICollection<Set> Sets { get; set; }

        public string Result
        {
            get
            {
                var first = 0;
                var second = 0;

                foreach (var set in Sets)
                {
                    if (set.FirstPlayerScore > set.SecondPlayerScore)
                    {
                        first++;
                    }
                    else
                    {
                        second++;
                    }
                }

                return $"{first}:{second}";
            }
        }
    }
}
