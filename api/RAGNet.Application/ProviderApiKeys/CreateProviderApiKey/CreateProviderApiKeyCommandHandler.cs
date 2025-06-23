using RAGNET.Application.Configuration.Commands;
using RAGNET.Domain.ProvidersApiKeys;
using RAGNET.Domain.SeedWork;
using RAGNET.Domain.SharedKernel.Providers;

namespace RAGNET.Application.ProviderApiKeys.CreateProviderApiKey
{
    public class CreateProviderApiKeyCommandHandlero(
        IProviderApiKeyRepository providerApiKeyRepository,
        ICryptoService cryptoService,
        IUnitOfWork unitOfWork,
        IProviderPolicyFactory providerPolicyFactory
    ) : ICommandHandler<CreateProviderApiKeyCommand, ProviderApiKeyDTO>
    {
        private readonly IProviderApiKeyRepository _providerApiKeyRepository = providerApiKeyRepository;
        private readonly ICryptoService _cryptoService = cryptoService;
        private readonly IProviderPolicyFactory _providerPolicyFactory = providerPolicyFactory;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        public async Task<ProviderApiKeyDTO> Handle(CreateProviderApiKeyCommand request, CancellationToken cancellationToken)
        {
            try
            {
                // Validate ApiKey
                var policy = _providerPolicyFactory.GetPolicy(request.Provider);
                policy.Validate(request.ApiKey);

                // Hash
                var encryptedApiKey = _cryptoService.Encrypt(request.ApiKey);

                // Create encrypted provider
                var provider = new Provider(providerId: request.Provider, apiKeyValue: encryptedApiKey, validate: false);
                var providerApiKey = ProviderApiKey.Create(
                    request.User.Id,
                    provider
                );

                await _providerApiKeyRepository.AddAsync(providerApiKey);
                await _unitOfWork.CommitAsync(cancellationToken);
                return providerApiKey.ToDTO();
            }
            catch (Exception)
            {
                await _unitOfWork.RevertAsync();
                throw;
            }
        }
    }
}