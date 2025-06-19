using RAGNET.Application.Configuration.Commands;
using RAGNET.Domain.ProvidersApiKeys;
using RAGNET.Domain.SeedWork;
using RAGNET.Domain.SharedKernel.Providers;

namespace RAGNET.Application.ProviderApiKeys.UpdateProviderApiKey
{
    public class UpdateProviderApiKeyCommandHandler(
        IProviderApiKeyRepository providerApiKeyRepository,
        ICryptoService cryptoService,
        IProviderPolicyFactory providerPolicyFactory,
        IUnitOfWork unitOfWork
    ) : ICommandHandler<UpdateProviderApiKeyCommand, ProviderApiKeyDTO>
    {
        private readonly IProviderApiKeyRepository _providerApiKeyRepository = providerApiKeyRepository;

        private readonly ICryptoService _cryptoService = cryptoService;
        private readonly IProviderPolicyFactory _providerPolicyFactory = providerPolicyFactory;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<ProviderApiKeyDTO> Handle(UpdateProviderApiKeyCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var userApiKey = await _providerApiKeyRepository.GetByIdAsync(request.ProviderApiKeyId, request.UserId) ?? throw new Exception("User API key not found");

                var encryptedApiKey = _cryptoService.Encrypt(request.ApiKey);
                var policy = _providerPolicyFactory.GetPolicy(userApiKey.Provider.ProviderType);

                policy.Validate(request.ApiKey);

                userApiKey.Provider = new Provider(
                    userApiKey.Provider.ProviderType,
                    encryptedApiKey,
                    policy,
                    false
                );

                await _providerApiKeyRepository.UpdateAsync(userApiKey, request.UserId);
                await _unitOfWork.CommitAsync();
                return userApiKey.ToDTO();
            }
            catch (Exception ex)
            {
                await _unitOfWork.RevertAsync();
                throw new Exception("Error updating user API key", ex);
            }
        }
    }
}