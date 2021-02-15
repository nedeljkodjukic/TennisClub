using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using TennisClub.Api.Models.Mongo;

namespace TennisClub.Api.Models.Input
{
    public class PlayerInputModel
    {
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime BirthDate { get; set; }

        [Required]
        public Country Country { get; set; }

        [Required]
        public string Bio { get; set; }

        [Required]
        public string StrongHand { get; set; }

        [Required]
        public string RacketBrand { get; set; }

        [Required]
        public int Height { get; set; }

        [Required]
        public int Weight { get; set; }

        [Required]
        public string Picture { get; set; }

        [Required]
        public string StandingPicture { get; set; }

        [Required]
        public ICollection<SocialNetworkAccount> Accounts { get; set; }
    }
}
