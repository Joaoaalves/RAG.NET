using RAGNET.Domain.Factories;
using RAGNET.Infrastructure.Factories;

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
            services.AddScoped<IContentFilterFactory, ContentFilterFactory>();
            return services;
        }
    }
}