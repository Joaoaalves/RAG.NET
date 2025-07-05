using RAGNET.Infrastructure.Workers.Handlers;

namespace web.Configurations
{
    public static class HandlerConfiguration
    {
        public static IServiceCollection AddHandlerConfiguration(this IServiceCollection services)
        {

            services.AddScoped<InitializeJobHandler>();
            services.AddScoped<ExtractTextHandler>();
            services.AddScoped<MountChunkerHandler>();
            services.AddScoped<MountProvidersHandler>();
            services.AddScoped<ProcessPagesHandler>();
            services.AddScoped<ConsumeTokensHandler>();
            services.AddScoped<StoreChunksHandler>();
            services.AddScoped<UpdateWorkflowHandler>();
            services.AddScoped<NotifyHandler>();
            return services;
        }
    }
}