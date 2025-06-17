using Microsoft.EntityFrameworkCore;
using RAGNET.Domain.SeedWork;
using RAGNET.Domain.SharedKernel.Providers;
using RAGNET.Infrastructure.Database;
using RAGNET.Infrastructure.Domain;
using RAGNET.Infrastructure.Providers;

namespace web.Extensions
{
    public static class DatabaseExtensions
    {
        public static IServiceCollection AddDatabaseConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseNpgsql(connectionString));

            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddSingleton<IProviderPolicyFactory, ProviderPolicyFactory>();

            return services;
        }
    }
}