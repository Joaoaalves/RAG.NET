using RAGNET.Application.Interfaces;
using RAGNET.Application.Services;
using RAGNET.Domain.Services;
using RAGNET.Domain.Services.Query;
using RAGNET.Infrastructure.Adapters.Document;
using RAGNET.Infrastructure.Services;

namespace web.Configurations
{
    public static class ServiceConfiguration
    {
        public static IServiceCollection AddServiceConfiguration(this IServiceCollection services)
        {
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IDocumentProcessingService, PdfProcessingAdapter>();
            services.AddScoped<IEmbeddingProviderValidator, EmbeddingProviderValidator>();
            services.AddScoped<IConversationProviderValidator, ConversationProviderValidator>();
            services.AddScoped<IPromptService, PromptService>();
            services.AddScoped<IQueryResultAggregatorService, QueryResultAggregatorService>();
            services.AddScoped<IEmbeddingProcessingService, EmbeddingProcessingService>();
            services.AddScoped<IChunkRetrieverService, ChunkRetrieverService>();
            services.AddScoped<IScoreNormalizerService, ScoreNormalizerService>();
            return services;
        }
    }
}