using RAGNET.Application.Filters;

namespace web.Configurations
{
    public static class FilterConfiguration
    {
        public static IServiceCollection AddFilterConfiguration(this IServiceCollection services)
        {
            services.AddScoped<ApiWorkflowFilter>();
            services.AddScoped<WebWorkflowFilter>();
            return services;
        }
    }
}