using RAGNET.Domain.VectorStorages;

namespace RAGNET.Application.Infrastructure.Providers.VectorDatabases
{
    public interface IVectorDatabaseFactory
    {
        Task<IVectorDatabaseService> CreateVectorDatabaseServiceAsync(VectorStorageId id, string userId);
    }
}