using RAGNET.Domain.Services;
using RAGNET.Infrastructure.Adapters.VectorDB;

namespace web.Configurations
{
    public static class AdapterConfiguration
    {
        public static IServiceCollection AddAdapterConfiguration(this IServiceCollection services)
        {

            services.AddScoped<IVectorDatabaseService, QDrantAdapter>();
            return services;
        }
    }
}