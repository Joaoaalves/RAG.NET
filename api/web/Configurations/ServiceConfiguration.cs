using RAGNET.Infrastructure.Services;

using RAGNET.Application.Interfaces;
using RAGNET.Application.Services;

using RAGNET.Domain.Services;
using RAGNET.Domain.Services.ApiKey;
using RAGNET.Domain.Services.Query;
using RAGNET.Domain.Services.Queue;

namespace web.Configurations
{
    public static class ServiceConfiguration
    {
        public static IServiceCollection AddServiceConfiguration(this IServiceCollection services)
        {
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IEmbeddingProviderResolver, EmbeddingProviderResolver>();
            services.AddScoped<IConversationProviderResolver, ConversationProviderResolver>();
            services.AddScoped<IPromptService, PromptService>();
            services.AddScoped<IQueryResultAggregatorService, QueryResultAggregatorService>();
            services.AddScoped<IEmbeddingProcessingService, EmbeddingProcessingService>();
            services.AddScoped<IChunkRetrieverService, ChunkRetrieverService>();
            services.AddScoped<IScoreNormalizerService, ScoreNormalizerService>();
            services.AddScoped<ICallbackNotificationService, CallbackNotificationService>();

            // ApiKey
            services.AddScoped<IApiKeyResolverService, ApiKeyResolverService>();
            services.AddScoped<ICryptoService, CryptoService>();
            return services;
        }
    }
}