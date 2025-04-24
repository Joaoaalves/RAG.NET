using RAGNET.Domain.Repositories;
using RAGNET.Domain.Services;
using RAGNET.Domain.Services.Queue;
using RAGNET.Infrastructure.Adapters.Queue;
using RAGNET.Infrastructure.Adapters.VectorDB;
using StackExchange.Redis;

namespace web.Configurations
{
    public static class AdapterConfiguration
    {
        public static IServiceCollection AddAdapterConfiguration(this IServiceCollection services, IConfiguration configuration)
        {

            services.AddSingleton<IConnectionMultiplexer>(sp =>
            {
                var configurationOptions = configuration.GetConnectionString("Redis") ?? throw new Exception("Redis connection string is obrigatory!");
                return ConnectionMultiplexer.Connect(configurationOptions);
            });

            services.AddSingleton<IEmbeddingJobQueue>(sp =>
                RabbitMqEmbeddingJobQueue
                    .CreateAsync(sp.GetRequiredService<IConfiguration>())
                    .GetAwaiter()
                    .GetResult()
            );

            services.AddScoped<IVectorDatabaseService, QDrantAdapter>();
            services.AddScoped<IJobStatusRepository, RedisJobStatusRepository>();

            return services;
        }
    }
}