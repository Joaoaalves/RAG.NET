using RAGNET.Infrastructure.ChatCompletions;
using RAGNET.Infrastructure.DocumentProcessors;
using RAGNET.Infrastructure.Embedders;

using RAGNET.Application.Infrastructure.Providers.Conversation;
using RAGNET.Application.Infrastructure.Providers.Embedding;
using RAGNET.Application.QueryEnhancers.Factories;
using RAGNET.Application.Chunkers.Factories;
using RAGNET.Application.QueryResultFilters.Factories;

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