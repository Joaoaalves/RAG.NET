using RAGNET.Domain.Repositories;
using RAGNET.Domain.Services;
using RAGNET.Infrastructure.Adapters.Queue;
using RAGNET.Infrastructure.Adapters.VectorDB;

namespace web.Configurations
{
    public static class AdapterConfiguration
    {
        public static IServiceCollection AddAdapterConfiguration(this IServiceCollection services, IConfiguration configuration)
        {

            services.AddScoped<IVectorDatabaseService, QDrantAdapter>();
            services.AddScoped<IJobStatusRepository, RedisJobStatusRepository>();
            return services;
        }
    }
}