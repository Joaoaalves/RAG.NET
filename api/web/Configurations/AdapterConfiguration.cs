using Confluent.Kafka;
using RAGNET.Domain.Services;
using RAGNET.Infrastructure.Adapters.VectorDB;

namespace web.Configurations
{
    public static class AdapterConfiguration
    {
        public static IServiceCollection AddAdapterConfiguration(this IServiceCollection services, IConfiguration configuration)
        {

            services.AddScoped<IVectorDatabaseService, QDrantAdapter>();
            services.AddSingleton(new ProducerConfig
            {
                BootstrapServers = configuration["Kafka:BootstrapServers"]
            });

            return services;
        }
    }
}