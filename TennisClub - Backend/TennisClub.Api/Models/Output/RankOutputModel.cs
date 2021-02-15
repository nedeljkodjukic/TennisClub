using TennisClub.Api.Models.Mongo;

namespace TennisClub.Api.Models.Output
{
    public class RankOutputModel
    {
        public string PlayerId { get; set; }

        public string Picture { get; set; }

        public Country Country { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public int Points { get; set; }

        public int Rank { get; set; }

        public int PreviousRank { get; set; }
    }
}
