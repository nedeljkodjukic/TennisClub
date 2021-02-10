

using System.ComponentModel.DataAnnotations;

namespace TennisClub.Api.Models.Identity
{
    public class RegisterInputModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Compare(nameof(Password), ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "The {0} must be at max {1} characters long.")]
        public string FirstName { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "The {0} must be at max {1} characters long.")]
        public string LastName { get; set; }

        [Required]
        [StringLength(30, ErrorMessage = "The {0} must be at max {1} characters long.")]
        public string UserName { get; set; }
    }
}
