using System;

namespace TennisClub.Api.Models.Identity
{
    public class TokenOutputModel
    {
        public string AccessToken { get; set; }
        public DateTime Expires { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
    }
}
