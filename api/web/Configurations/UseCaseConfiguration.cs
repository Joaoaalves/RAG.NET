using RAGNET.Application.UseCases.QueryResultFilterUseCases;
using RAGNET.Application.UseCases.Query;

namespace web.Configurations
{
    public static class UseCaseConfiguration
    {
        public static IServiceCollection AddUseCaseConfiguration(this IServiceCollection services)
        {
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