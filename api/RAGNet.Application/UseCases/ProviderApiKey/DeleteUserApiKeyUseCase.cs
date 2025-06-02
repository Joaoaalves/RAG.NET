using RAGNET.Domain.Repositories;

namespace RAGNET.Application.UseCases.ProviderApiKey
{
    public interface IDeleteProviderApiKeyUseCase
    {
        Task<bool> ExecuteAsync(Guid userApiKeyId, string userId);
    }

    public class DeleteProviderApiKeyUseCase(
        IProviderApiKeyRepository userApiKeyRepository
    ) : IDeleteProviderApiKeyUseCase
    {
        private readonly IProviderApiKeyRepository _userApiKeyRepository = userApiKeyRepository;

        public async Task<bool> ExecuteAsync(Guid userApiKeyId, string userId)
        {
            try
            {
                var userApiKey = await _userApiKeyRepository.GetByIdAsync(userApiKeyId, userId) ??
                    throw new Exception("User API key not found");

                await _userApiKeyRepository.DeleteAsync(userApiKey, userId);

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