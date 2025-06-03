using RAGNET.Application.UseCases.CallbackUrlUseCases;
using RAGNET.Application.UseCases.ContentFilterUseCases;
using RAGNET.Application.UseCases.Query;
using RAGNET.Application.UseCases.QueryEnhancerUseCases;
using RAGNET.Application.UseCases.ProviderApiKeyUseCases;
using RAGNET.Application.UseCases.WorkflowUseCases;

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

            // Workflow
            services.AddScoped<IGetWorkflowUseCase, GetWorkflowUseCase>();
            services.AddScoped<ICreateWorkflowUseCase, CreateWorkflowUseCase>();
            services.AddScoped<IGetUserWorkflowsUseCase, GetUserWorkflowsUseCase>();
            services.AddScoped<IDeleteWorkflowUseCase, DeleteWorkflowUseCase>();
            services.AddScoped<IUpdateWorkflowUseCase, UpdateWorkflowUseCase>();

            // Query Enhancer
            services.AddScoped<ICreateQueryEnhancerUseCase, CreateQueryEnhancerUseCase>();
            services.AddScoped<IUpdateQueryEnhancerUseCase, UpdateQueryEnhancerUseCase>();
            services.AddScoped<IDeleteQueryEnhancerUseCase, DeleteQueryEnhancerUseCase>();
            services.AddScoped<IEnhanceQueryUseCase, EnhanceQueryUseCase>();

            // Chunk
            services.AddScoped<IQueryChunksUseCase, QueryChunksUseCase>();

            // Query
            services.AddScoped<IProcessQueryUseCase, ProcessQueryUseCase>();

            // Filter
            services.AddScoped<IFilterContentUseCase, FilterContentUseCase>();
            services.AddScoped<ICreateContentFilterUseCase, CreateContentFilterUseCase>();
            services.AddScoped<IUpdateContentFilterUseCase, UpdateContentFilterUseCase>();
            services.AddScoped<IDeleteContentFilterUseCase, DeleteContentFilterUseCase>();

            // Callback URL
            services.AddScoped<IAddCallbackUrlUseCase, AddCallbackUrlUseCase>();
            services.AddScoped<IUpdateCallbackUrlUseCase, UpdateCallbackUrlUseCase>();
            services.AddScoped<IDeleteCallbackUrlUseCase, DeleteCallbackUrlUseCase>();

            return services;
        }
    }
}