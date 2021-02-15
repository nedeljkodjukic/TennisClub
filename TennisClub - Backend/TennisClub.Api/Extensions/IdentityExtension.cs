using AspNetCore.Identity.Mongo;
using AspNetCore.Identity.Mongo.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TennisClub.Api.Models.Mongo;

namespace TennisClub.Api.Extensions
{
    public static class IdentityExtension
    {
        public static IdentityBuilder AddMongoDbProvider(this IServiceCollection services, IConfiguration configuration)
        {
            string connectionString = configuration["DatabaseSettings:ConnectionString"];
            string dbName = configuration["DatabaseSettings:DatabaseName"];

            
            return services.AddIdentityMongoDbProvider<ApplicationUser, MongoRole>(options =>
            {
                options.Password.RequiredLength = 8;
                options.Password.RequireLowercase = true;
                options.Password.RequireUppercase = true;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequireDigit = true;
                options.User.RequireUniqueEmail = true;
            }, opt => opt.ConnectionString = $"{connectionString}/{dbName}");
            
        }
    }
}
