using AspNetCore.Identity.Mongo.Model;

namespace TennisClub.Api.Models.Mongo
{
    public class ApplicationUser : MongoUser
    {
        public ApplicationUser(string userName)
            : base(userName)
        {

        }

        public string FirstName { get; set; }

        public string LastName { get; set; }
    }
}
