using Microsoft.Extensions.DependencyInjection;
using TennisClub.Api.Services;

namespace TennisClub.Api.Extensions
{
    public static class ServicesDependenciesExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            return services.AddScoped<IdentityService>();
        }
    }
}
