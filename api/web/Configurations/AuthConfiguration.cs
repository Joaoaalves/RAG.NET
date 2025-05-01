using Microsoft.AspNetCore.Authentication.BearerToken;
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

            services.ConfigureAll<BearerTokenOptions>(options =>
            {
                options.BearerTokenExpiration = TimeSpan.FromMinutes(60);

                options.Events.OnMessageReceived = context =>
                {
                    var accessToken = context.Request.Query["access_token"];
                    var path = context.HttpContext.Request.Path;

                    if (!string.IsNullOrEmpty(accessToken) &&
                        path.StartsWithSegments("/hubs/jobstatus"))
                    {
                        context.Token = accessToken;
                    }

                    return Task.CompletedTask;
                };
            });

            return services;
        }
    }
}
