using StackExchange.Redis;

using RAGNET.Domain.SharedKernel.Providers;

using RAGNET.Infrastructure.ChatCompletions;
using RAGNET.Infrastructure.SignalR;
using RAGNET.Infrastructure.Jobs.Queue;
using RAGNET.Infrastructure.Qdrant;
using RAGNET.Infrastructure.RabbitMQ;
using RAGNET.Infrastructure.Redis;
using RAGNET.Infrastructure.Trello;
using RAGNET.Infrastructure.Workers;
using RAGNET.Infrastructure.Jobs;
using RAGNET.Infrastructure.Embedders;

using RAGNET.Application.Feedbacks.Services;
using RAGNET.Application.Infrastructure.Providers;
using RAGNET.Application.Subscriptions.Services;
using RAGNET.Infrastructure.Payments;

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

            // REDIS
            services.AddScoped<IJobStatusRepository, RedisJobStatusRepository>();
            services.AddScoped<IPaymentStatusService, RedisPaymentStatusRepository>();

            // Trello
            services.AddScoped<ICardCreatorService, TrelloCardCreator>();

            // Stripe
            services.AddScoped<IPaymentGateway, StripeGateway>();
            services.Configure<StripeSettings>(configuration.GetSection("Stripe"));

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