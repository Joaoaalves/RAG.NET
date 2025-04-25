using RAGNet.Domain.Services;
using RAGNET.Domain.Contexts;
using RAGNET.Infrastructure.Workers.Handlers;

namespace web.Configurations
{
    public static class HandlerConfiguration
    {
        public static IServiceCollection AddHandlerConfiguration(this IServiceCollection services)
        {

            services.AddScoped<InitializeJobHandler>();
            services.AddScoped<ExtractTextHandler>();
            services.AddScoped<ProcessPagesHandler>();
            services.AddScoped<UpdateWorkflowHandler>();
            services.AddScoped<NotifyHandler>();
            return services;
        }
    }
}