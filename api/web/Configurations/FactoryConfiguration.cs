using RAGNET.Application.Chunkers;
using RAGNET.Application.UserQueriesEnhancers;

using RAGNET.Infrastructure.ChatCompletions;
using RAGNET.Infrastructure.DocumentProcessors;
using RAGNET.Application.Providers;
using RAGNET.Infrastructure.Embedders;
using RAGNET.Application.UserQueriesResultFilters;

namespace web.Configurations
{
    public static class FactoryConfiguration
    {
        public static IServiceCollection AddFactoryConfiguration(this IServiceCollection services)
        {
            services.AddScoped<ITextChunkerFactory, TextChunkerFactory>();
            services.AddScoped<IChatCompletionFactory, ChatCompletionFactory>();
            services.AddScoped<IEmbedderFactory, EmbedderFactory>();
            services.AddScoped<IQueryEnhancerFactory, QueryEnhancerFactory>();
            services.AddScoped<IQueryResultFilterFactory, QueryResultFilterFactory>();
            services.AddScoped<IDocumentProcessorFactory, DocumentProcessorFactory>();
            return services;
        }
    }
}