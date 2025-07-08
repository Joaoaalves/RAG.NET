using RAGNET.Domain.SharedKernel.VectorStorages;

namespace RAGNET.Domain.VectorStorages
{
    public interface IVectorStorageRepository
    {
        Task<VectorStorage?> GetByIdAsync(VectorStorageId id, string userId);
        Task<VectorStorage> AddAsync(VectorStorage entity);
        Task UpdateAsync(VectorStorage entity, string userId);
        Task DeleteAsync(VectorStorage entity, string userId);
        Task<IEnumerable<VectorStorage>> GetByUserIdAsync(string userId);
        Task<VectorStorage?> GetByUserIdAndProviderAsync(string userId, VectorStorageProvider provider);
    }
}