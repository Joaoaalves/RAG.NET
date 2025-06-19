using RAGNET.Application.UseCases.QueryResultFilterUseCases;
using RAGNET.Application.UseCases.Query;
using RAGNET.Application.UseCases.QueryEnhancerUseCases;
using RAGNET.Application.UseCases.ProviderApiKeyUseCases;

namespace web.Configurations
{
    public static class UseCaseConfiguration
    {
        public static IServiceCollection AddUseCaseConfiguration(this IServiceCollection services)
        {

            // User API Key
            services.AddScoped<ICreateProviderApiKeyUseCase, CreateProviderApiKeyUseCase>();
            services.AddScoped<IGetProviderApiKeysUseCase, GetProviderApiKeysUseCase>();
            services.AddScoped<IUpdateProviderApiKeyUseCase, UpdateProviderApiKeyUseCase>();
            services.AddScoped<IDeleteProviderApiKeyUseCase, DeleteProviderApiKeyUseCase>();

            // Query Enhancer
            services.AddScoped<ICreateQueryEnhancerUseCase, CreateQueryEnhancerUseCase>();
            services.AddScoped<IUpdateQueryEnhancerUseCase, UpdateQueryEnhancerUseCase>();
            services.AddScoped<IDeleteQueryEnhancerUseCase, DeleteQueryEnhancerUseCase>();
            services.AddScoped<IEnhanceQueryUseCase, EnhanceQueryUseCase>();

            // Chunk
            services.AddScoped<IQueryChunksUseCase, QueryChunksUseCase>();

            // Query
            services.AddScoped<IProcessQueryUseCase, ProcessQueryUseCase>();

            // QueryResultFilter
            services.AddScoped<IFilterContentUseCase, FilterContentUseCase>();
            services.AddScoped<ICreateQueryResultFilterUseCase, CreateQueryResultFilterUseCase>();
            services.AddScoped<IUpdateQueryResultFilterUseCase, UpdateQueryResultFilterUseCase>();
            services.AddScoped<IDeleteQueryResultFilterUseCase, DeleteQueryResultFilterUseCase>();

            return services;
        }
    }
}