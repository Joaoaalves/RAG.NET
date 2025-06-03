using RAGNET.Domain.Repositories;
using RAGNET.Domain.Services;
using RAGNET.Domain.Services.Queue;
using RAGNET.Domain.SharedKernel.Providers;
using RAGNET.Infrastructure.Adapters.Chat.Anthropic;
using RAGNET.Infrastructure.Adapters.Chat.Gemini;
using RAGNET.Infrastructure.Adapters.Chat.OpenAi;
using RAGNET.Infrastructure.Adapters.Embedding.Gemini;
using RAGNET.Infrastructure.Adapters.Embedding.OpenAI;
using RAGNET.Infrastructure.Adapters.Embedding.Voyage;
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


            // Providers
            services.AddSingleton<OpenAIChatModelCatalog>();
            services.AddSingleton<AnthropicChatModelCatalog>();
            services.AddSingleton<GeminiChatModelCatalog>();
            services.AddSingleton<Dictionary<SupportedProvider, IProviderConversationModelCatalog>>(sp => new()
            {
                { SupportedProvider.OpenAI, sp.GetRequiredService<OpenAIChatModelCatalog>() },
                { SupportedProvider.Anthropic, sp.GetRequiredService<AnthropicChatModelCatalog>() },
                { SupportedProvider.Gemini, sp.GetRequiredService<GeminiChatModelCatalog>() },
            });


            services.AddSingleton<OpenAIEmbeddingModelCatalog>();
            services.AddSingleton<VoyageEmbeddingModelCatalog>();
            services.AddSingleton<GeminiEmbeddingModelCatalog>();

            services.AddSingleton<Dictionary<SupportedProvider, IProviderEmbeddingModelCatalog>>(sp => new()
            {
                { SupportedProvider.OpenAI, sp.GetRequiredService<OpenAIEmbeddingModelCatalog>() },
                { SupportedProvider.Voyage, sp.GetRequiredService<VoyageEmbeddingModelCatalog>() },
                { SupportedProvider.Gemini, sp.GetRequiredService<GeminiEmbeddingModelCatalog>() },
            });

            return services;
        }
    }
}