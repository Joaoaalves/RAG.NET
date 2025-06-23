using RAGNET.Application.Chunkers;

using RAGNET.Infrastructure.ChatCompletions;
using RAGNET.Infrastructure.DocumentProcessors;
using RAGNET.Infrastructure.Embedders;

using RAGNET.Application.QueryResultFilters;
using RAGNET.Application.Providers.Conversation;
using RAGNET.Application.Providers.Embedding;
using RAGNET.Application.QueryEnhancers;

namespace web.Configurations
{
    public static class FactoryConfiguration
    {
        public static IServiceCollection AddFactoryConfiguration(this IServiceCollection services)
        {
            services.AddScoped<ITextChunkerFactory, TextChunkerFactory>();
            services.AddScoped<IConversationProviderFactory, ChatCompletionFactory>();
            services.AddScoped<IEmbedderFactory, EmbedderFactory>();
            services.AddScoped<IQueryEnhancerFactory, QueryEnhancerFactory>();
            services.AddScoped<IQueryResultFilterFactory, QueryResultFilterFactory>();
            services.AddScoped<IDocumentProcessorFactory, DocumentProcessorFactory>();
            return services;
        }
    }
}