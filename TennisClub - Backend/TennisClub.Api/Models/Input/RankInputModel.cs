using System.ComponentModel.DataAnnotations;

namespace TennisClub.Api.Models.Input
{
    public class RankInputModel
    {

        [Required]
        public string PlayerId { get; set; }


        [Required]
        public int RankNumber { get; set; }


        [Required]
        public int PreviousRankNumber { get; set; }


        [Required]
        public int Points { get; set; }
    }
}
