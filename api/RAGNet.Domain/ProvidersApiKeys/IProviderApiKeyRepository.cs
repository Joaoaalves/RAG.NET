using RAGNET.Domain.SharedKernel.Providers;

namespace RAGNET.Domain.ProvidersApiKeys
{
    public interface IProviderApiKeyRepository
    {
        Task<ProviderApiKey?> GetByIdAsync(ProviderApiKeyId id, string? userId);
        Task<ProviderApiKey> AddAsync(ProviderApiKey entity);
        Task UpdateAsync(ProviderApiKey entity, string? userId);
        Task DeleteAsync(ProviderApiKey entity, string? userId);
        Task<IEnumerable<ProviderApiKey>> GetByUserIdAsync(string userId);
        Task<ProviderApiKey?> GetByUserIdAndProviderAsync(string userId, SupportedProvider provider);
    }
}