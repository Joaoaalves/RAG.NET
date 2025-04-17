using RAGNET.Domain.Repositories;

namespace RAGNET.Application.UseCases.UserApiKey
{
    public interface IDeleteUserApiKeyUseCase
    {
        Task<bool> ExecuteAsync(Guid userApiKeyId, string userId);
    }

    public class DeleteUserApiKeyUseCase(
        IUserApiKeyRepository userApiKeyRepository
    ) : IDeleteUserApiKeyUseCase
    {
        private readonly IUserApiKeyRepository _userApiKeyRepository = userApiKeyRepository;

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