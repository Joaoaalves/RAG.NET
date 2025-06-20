using RAGNET.Application.UseCases.QueryResultFilterUseCases;

namespace web.Configurations
{
    public static class UseCaseConfiguration
    {
        public static IServiceCollection AddUseCaseConfiguration(this IServiceCollection services)
        {
            // QueryResultFilter
            services.AddScoped<ICreateQueryResultFilterUseCase, CreateQueryResultFilterUseCase>();
            services.AddScoped<IUpdateQueryResultFilterUseCase, UpdateQueryResultFilterUseCase>();
            services.AddScoped<IDeleteQueryResultFilterUseCase, DeleteQueryResultFilterUseCase>();

            return services;
        }
    }
}