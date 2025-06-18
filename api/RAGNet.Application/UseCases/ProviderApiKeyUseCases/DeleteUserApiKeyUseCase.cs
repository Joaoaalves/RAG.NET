using RAGNET.Domain.ProvidersApiKeys;
using RAGNET.Domain.SeedWork;

namespace RAGNET.Application.UseCases.ProviderApiKeyUseCases
{
    public interface IDeleteProviderApiKeyUseCase
    {
        Task<bool> ExecuteAsync(ProviderApiKeyId userApiKeyId, string userId);
    }

    public class DeleteProviderApiKeyUseCase(
        IUnitOfWork unitOfWork,
        IProviderApiKeyRepository providerApiKeyRepository
    ) : IDeleteProviderApiKeyUseCase
    {
        private readonly IProviderApiKeyRepository _providerApiKeyRepository = providerApiKeyRepository;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        public async Task<bool> ExecuteAsync(ProviderApiKeyId userApiKeyId, string userId)
        {
            try
            {
                var userApiKey = await _providerApiKeyRepository.GetByIdAsync(userApiKeyId, userId) ??
                    throw new Exception("User API key not found");

                await _providerApiKeyRepository.DeleteAsync(userApiKey, userId);
                await _unitOfWork.CommitAsync();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                await _unitOfWork.RevertAsync();
                throw new Exception("Error deleting user API key", ex);
            }
        }
    }

}