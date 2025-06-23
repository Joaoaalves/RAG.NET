using RAGNET.Application.Configuration.Commands;
using RAGNET.Domain.ProvidersApiKeys;
using RAGNET.Domain.SeedWork;

namespace RAGNET.Application.ProviderApiKeys.Commands.DeleteProviderApiKey
{
    public class DeleteProviderApiKeyCommandHandler(
        IUnitOfWork unitOfWork,
        IProviderApiKeyRepository providerApiKeyRepository
    ) : ICommandHandler<DeleteProviderApiKeyCommand, bool>
    {
        private readonly IProviderApiKeyRepository _providerApiKeyRepository = providerApiKeyRepository;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        public async Task<bool> Handle(DeleteProviderApiKeyCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var userApiKey = await _providerApiKeyRepository.GetByIdAsync(request.ProviderApiKeyId, request.User.Id) ??
                    throw new Exception("User API key not found");

                await _providerApiKeyRepository.DeleteAsync(userApiKey, request.User.Id);
                await _unitOfWork.CommitAsync(cancellationToken);
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