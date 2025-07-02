using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using RAGNET.Infrastructure.Database;
using RAGNET.Infrastructure.SeedWork;

namespace tests.RAGNet.Infrastructure.Tests.Database
{
    public static class TestDbContextFactory
    {
        public static ApplicationDbContext CreateInMemoryContext(string? dbName = null)
        {
            var services = new ServiceCollection();

            services.AddEntityFrameworkInMemoryDatabase();
            services.Replace(ServiceDescriptor.Singleton<IValueConverterSelector, StronglyTypedIdValueConverterSelector>());

            var serviceProvider = services.BuildServiceProvider();

            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(dbName ?? Guid.NewGuid().ToString())
                .UseInternalServiceProvider(serviceProvider)
                .Options;

            return new ApplicationDbContext(options);
        }
    }
}
