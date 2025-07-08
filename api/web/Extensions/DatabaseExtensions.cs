using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using RAGNET.Domain.SeedWork;
using RAGNET.Domain.SharedKernel.Providers;
using RAGNET.Domain.SharedKernel.VectorStorages;
using RAGNET.Infrastructure.Database;
using RAGNET.Infrastructure.Domain;
using RAGNET.Infrastructure.Providers;
using RAGNET.Infrastructure.SeedWork;
using RAGNET.Infrastructure.VectorDatabases;

namespace web.Extensions
{
    public static class DatabaseExtensions
    {
        public static IServiceCollection AddDatabaseConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");

            services.AddDbContext<ApplicationDbContext>((serviceProvider, options) =>
            {
                options.UseNpgsql(connectionString, npgsqlOptions => { });

                // Strongly Typed Id Converter
                options.ReplaceService<IValueConverterSelector, StronglyTypedIdValueConverterSelector>();
            });

            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddSingleton<IProviderPolicyFactory, ProviderPolicyFactory>();
            services.AddSingleton<IVectorStoragePolicyFactory, VectorStoragePolicyFactory>();

            return services;
        }
    }
}