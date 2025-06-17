using RAGNET.Domain.SharedKernel.Providers;

using RAGNET.Application.Interfaces;
using RAGNET.Application.Auth;
using RAGNET.Application.Query;
using RAGNET.Application.Providers;
using RAGNET.Application.Chunkers;
using RAGNET.Application.ApiKeys;

using RAGNET.Infrastructure.Embedders;
using RAGNET.Infrastructure.ChatCompletions;
using RAGNET.Infrastructure.Providers;

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

            // ApiKey
            services.AddScoped<IApiKeyResolverService, ApiKeyResolverService>();
            services.AddScoped<ICryptoService, CryptoService>();

            services.AddSingleton<IProviderModelCatalogService, ProviderModelCatalogService>();

            return services;
        }
    }
}