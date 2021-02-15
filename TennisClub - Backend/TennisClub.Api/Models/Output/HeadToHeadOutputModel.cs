using System.Collections.Generic;
using System.Linq;

namespace TennisClub.Api.Models.Output
{
    public class HeadToHeadOutputModel
    {
        public IEnumerable<MatchOutputModel> RecentMatches { get; set; }

        public Dictionary<string, int> FirstPlayerWins { get; set; }

        public Dictionary<string, int> SecondPlayerWins { get; set; }

        public int FirstPlayerGrandSlamWins { get; set; }

        public int SecondPlayerGrandSlamWins { get; set; }

        public int FirstPlayerTotalWins
        {
            get
            {
                return FirstPlayerWins.Values.Sum();
            }
        }

        public int SecondPlayerTotalWins
        {
            get
            {
                return SecondPlayerWins.Values.Sum();
            }
        }

    }
}
