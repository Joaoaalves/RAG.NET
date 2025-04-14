using RAGNET.Application.UseCases.ContentFilterUseCases;
using RAGNET.Application.UseCases.EmbeddingUseCases;
using RAGNET.Application.UseCases.Query;
using RAGNET.Application.UseCases.QueryEnhancerUseCases;
using RAGNET.Application.UseCases.WorkflowUseCases;

namespace web.Configurations
{
    public static class UseCaseConfiguration
    {
        public static IServiceCollection AddUseCaseConfiguration(this IServiceCollection services)
        {
            services.AddScoped<IGetUserWorkflowsUseCase, GetUserWorkflowsUseCase>();
            services.AddScoped<IProcessEmbeddingUseCase, ProcessEmbeddingUseCase>();
            services.AddScoped<ICreateWorkflowUseCase, CreateWorkflowUseCase>();
            services.AddScoped<IGetWorkflowUseCase, GetWorkflowUseCase>();
            services.AddScoped<IDeleteWorkflowUseCase, DeleteWorkflowUseCase>();
            services.AddScoped<ICreateQueryEnhancerUseCase, CreateQueryEnhancerUseCase>();
            services.AddScoped<IUpdateQueryEnhancerUseCase, UpdateQueryEnhancerUseCase>();
            services.AddScoped<IDeleteQueryEnhancerUseCase, DeleteQueryEnhancerUseCase>();
            services.AddScoped<IEnhanceQueryUseCase, EnhanceQueryUseCase>();
            services.AddScoped<IQueryChunksUseCase, QueryChunksUseCase>();
            services.AddScoped<IProcessQueryUseCase, ProcessQueryUseCase>();
            services.AddScoped<IFilterContentUseCase, FilterContentUseCase>();
            services.AddScoped<ICreateContentFilterUseCase, CreateContentFilterUseCase>();
            services.AddScoped<IUpdateContentFilterUseCase, UpdateContentFilterUseCase>();
            services.AddScoped<IDeleteContentFilterUseCase, DeleteContentFilterUseCase>();
            return services;
        }
    }
}