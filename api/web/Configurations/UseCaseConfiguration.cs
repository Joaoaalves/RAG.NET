using RAGNET.Application.UseCases.CallbackUrlUseCases;
using RAGNET.Application.UseCases.ContentFilterUseCases;
using RAGNET.Application.UseCases.Query;
using RAGNET.Application.UseCases.QueryEnhancerUseCases;
using RAGNET.Application.UseCases.UserApiKey;
using RAGNET.Application.UseCases.WorkflowUseCases;

namespace web.Configurations
{
    public static class UseCaseConfiguration
    {
        public static IServiceCollection AddUseCaseConfiguration(this IServiceCollection services)
        {
            // User API Key
            services.AddScoped<ICreateUserApiKeyUseCase, CreateUserApiKeyUseCase>();
            services.AddScoped<IGetUserApiKeysUseCase, GetUserApiKeysUseCase>();
            services.AddScoped<IUpdateUserApiKeyUseCase, UpdateUserApiKeyUseCase>();
            services.AddScoped<IDeleteUserApiKeyUseCase, DeleteUserApiKeyUseCase>();

            // Workflow
            services.AddScoped<IGetWorkflowUseCase, GetWorkflowUseCase>();
            services.AddScoped<ICreateWorkflowUseCase, CreateWorkflowUseCase>();
            services.AddScoped<IGetUserWorkflowsUseCase, GetUserWorkflowsUseCase>();
            services.AddScoped<IDeleteWorkflowUseCase, DeleteWorkflowUseCase>();

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