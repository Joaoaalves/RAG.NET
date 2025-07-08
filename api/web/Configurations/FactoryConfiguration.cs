using RAGNET.Infrastructure.ChatCompletions;
using RAGNET.Infrastructure.DocumentProcessors;
using RAGNET.Infrastructure.Embedders;

using RAGNET.Application.Infrastructure.Providers.Conversation;
using RAGNET.Application.Infrastructure.Providers.Embedding;
using RAGNET.Application.QueryEnhancers.Factories;
using RAGNET.Application.Chunkers.Factories;
using RAGNET.Application.QueryResultFilters.Factories;
using RAGNET.Application.Infrastructure.Providers.VectorDatabases;
using RAGNET.Infrastructure.VectorDatabases;

namespace web.Configurations
{
    public static class FactoryConfiguration
    {
        public static IServiceCollection AddFactoryConfiguration(this IServiceCollection services)
        {
            services.AddScoped<ITextChunkerFactory, TextChunkerFactory>();
            services.AddScoped<IConversationProviderFactory, ConversationProviderFactory>();
            services.AddScoped<IEmbedderFactory, EmbedderFactory>();
            services.AddScoped<IQueryEnhancerFactory, QueryEnhancerFactory>();
            services.AddScoped<IQueryResultFilterFactory, QueryResultFilterFactory>();
            services.AddScoped<IDocumentProcessorFactory, DocumentProcessorFactory>();
            services.AddScoped<IVectorDatabaseFactory, VectorDatabaseFactory>();
            return services;
        }
    }
}