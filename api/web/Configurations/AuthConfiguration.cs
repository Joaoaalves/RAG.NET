using Microsoft.AspNetCore.Authentication.BearerToken;
using Microsoft.AspNetCore.Authorization;
using RAGNET.Domain.Entities;
using RAGNET.Infrastructure.Data;

namespace web.Configurations
{
    public static class AuthConfiguration
    {
        public static IServiceCollection AddAuthConfiguration(this IServiceCollection services)
        {
            services.AddIdentityApiEndpoints<User>()
    .AddEntityFrameworkStores<ApplicationDbContext>();

            services.ConfigureAll<BearerTokenOptions>(option =>
            {
                option.BearerTokenExpiration = TimeSpan.FromMinutes(60);
            });

            return services;
        }
    }
}