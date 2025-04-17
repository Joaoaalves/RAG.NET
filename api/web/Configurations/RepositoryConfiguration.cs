
using RAGNET.Domain.Repositories;
using RAGNET.Infrastructure.Repositories;

namespace web.Configurations
{
    public static class RepositoryConfiguration
    {
        public static IServiceCollection AddRepositoryConfiguration(this IServiceCollection services)
        {
            services.AddScoped<IWorkflowRepository, WorkflowRepository>();
            services.AddScoped<IUserApiKeyRepository, UserApiKeyRepository>();
            services.AddScoped<IChunkerRepository, ChunkerRepository>();
            services.AddScoped<IQueryEnhancerRepository, QueryEnhancerRepository>();
            services.AddScoped<IFilterRepository, FilterRepository>();
            services.AddScoped<IRankerRepository, RankerRepository>();
            services.AddScoped<IChunkRepository, ChunkRepository>();
            services.AddScoped<IEmbeddingProviderConfigRepository, EmbeddingProviderConfigRepository>();
            services.AddScoped<IConversationProviderConfigRepository, ConversationProviderConfigRepository>();
            services.AddScoped<IDocumentRepository, DocumentRepository>();
            services.AddScoped<IPageRepository, PageRepository>();
            return services;
        }
    }
}