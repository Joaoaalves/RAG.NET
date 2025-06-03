using RAGNET.Domain.Repositories;

namespace RAGNET.Application.UseCases.ProviderApiKeyUseCases
{
    public interface IDeleteProviderApiKeyUseCase
    {
        Task<bool> ExecuteAsync(Guid userApiKeyId, string userId);
    }

    public class DeleteProviderApiKeyUseCase(
        IProviderApiKeyRepository providerApiKeyRepository
    ) : IDeleteProviderApiKeyUseCase
    {
        private readonly IProviderApiKeyRepository _providerApiKeyRepository = providerApiKeyRepository;

        public async Task<bool> ExecuteAsync(Guid userApiKeyId, string userId)
        {
            try
            {
                var userApiKey = await _providerApiKeyRepository.GetByIdAsync(userApiKeyId, userId) ??
                    throw new Exception("User API key not found");

                await _providerApiKeyRepository.DeleteAsync(userApiKey, userId);

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw new Exception("Error deleting user API key", ex);
            }
        }
    }

}