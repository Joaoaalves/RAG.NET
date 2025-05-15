using RAGNET.Domain.Repositories;
using RAGNET.Domain.Services;
using RAGNET.Domain.Services.Queue;
using RAGNET.Infrastructure.Adapters.Queue;
using RAGNET.Infrastructure.Adapters.SignalR;
using RAGNET.Infrastructure.Adapters.Trello;
using RAGNET.Infrastructure.Adapters.VectorDB;
using RAGNET.Infrastructure.Services;
using RAGNET.Infrastructure.Workers;
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


            string host = configuration["RabbitMQ:Host"]
                ?? throw new Exception("RabbitMQ Host must be set.");
            string userName = configuration["RabbitMQ:Username"]
                ?? throw new Exception("RabbitMQ Username must be set.");
            string password = configuration["RabbitMQ:Password"]
                ?? throw new Exception("RabbitMQ Password must be set.");

            services.AddSingleton<IEmbeddingJobQueue>(sp =>
                RabbitMqEmbeddingJobQueue
                    .CreateAsync(host, userName, password)
                    .GetAwaiter()
                    .GetResult()
            );

            // Callback Service
            services.AddHttpClient("CallbackClient")
                .SetHandlerLifetime(TimeSpan.FromMinutes(5));
            services.AddSingleton(typeof(ICallbackNotificationService<>), typeof(CallbackNotificationService<>)); services.AddHostedService<EmbeddingJobWorker>();
            services.AddSingleton<IJobNotificationService, SignalRJobNotificationService>();

            services.AddScoped<IVectorDatabaseService, QDrantAdapter>();
            services.AddScoped<IJobStatusRepository, RedisJobStatusRepository>();


            // Trello
            services.AddScoped<ICardCreatorService, TrelloCardCreator>();

            return services;
        }
    }
}