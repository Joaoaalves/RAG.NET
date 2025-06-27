using RAGNET.Domain.Chunkers;
using RAGNET.Domain.Documents;
using RAGNET.Domain.QueryResultFilters;
using RAGNET.Domain.ProvidersApiKeys;
using RAGNET.Domain.QueryEnhancers;
using RAGNET.Domain.Rankers;
using RAGNET.Domain.Workflows;
using RAGNET.Domain.Workflows.CallbackUrls;

using RAGNET.Infrastructure.Domain.Chunkers;
using RAGNET.Infrastructure.Domain.Documents;
using RAGNET.Infrastructure.Domain.ProviderApiKeys;
using RAGNET.Infrastructure.Domain.QueryEnhancers;
using RAGNET.Infrastructure.Domain.Rankers;
using RAGNET.Infrastructure.Domain.Workflows;
using RAGNET.Infrastructure.Domain.QueryResultFilters;
using RAGNET.Infrastructure.Domain.Workflows.CallbackUrls;
using RAGNET.Domain.Documents.Pages.Chunks;
using RAGNET.Infrastructure.Domain.Documents.Pages.Chunks;
using RAGNET.Domain.Documents.Pages;
using RAGNET.Infrastructure.Domain.Documents.Pages;
using RAGNET.Domain.TokenWallets;
using RAGNET.Infrastructure.Domain.TokenWallets;
using RAGNET.Domain.Users.Subscriptions;
using RAGNET.Infrastructure.Domain.Subscriptions;

namespace web.Configurations
{
    public static class RepositoryConfiguration
    {
        public static IServiceCollection AddRepositoryConfiguration(this IServiceCollection services)
        {
            services.AddScoped<IWorkflowRepository, WorkflowRepository>();
            services.AddScoped<IProviderApiKeyRepository, ProviderApiKeyRepository>();
            services.AddScoped<IChunkerRepository, ChunkerRepository>();
            services.AddScoped<IQueryEnhancerRepository, QueryEnhancerRepository>();
            services.AddScoped<IQueryResultFilterRepository, QueryResultFilterRepository>();
            services.AddScoped<IRankerRepository, RankerRepository>();
            services.AddScoped<IChunkRepository, ChunkRepository>();
            services.AddScoped<IDocumentRepository, DocumentRepository>();
            services.AddScoped<IPageRepository, PageRepository>();
            services.AddScoped<ICallbackUrlRepository, CallbackUrlRepository>();
            services.AddScoped<ITokenWalletRepository, TokenWalletRepository>();
            services.AddScoped<ISubscriptionRepository, SubscriptionRepository>();
            return services;
        }
    }
}