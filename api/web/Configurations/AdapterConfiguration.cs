using RAGNET.Domain.Services;
using RAGNET.Domain.Services.Queue;
using RAGNET.Infrastructure.Adapters.Queue;
using RAGNET.Infrastructure.Adapters.VectorDB;

namespace web.Configurations
{
    public static class AdapterConfiguration
    {
        public static IServiceCollection AddAdapterConfiguration(this IServiceCollection services, IConfiguration configuration)
        {

            services.AddScoped<IVectorDatabaseService, QDrantAdapter>();
            services.AddScoped<IJobQueueService, RedisJobQueueService>();
            return services;
        }
    }
}