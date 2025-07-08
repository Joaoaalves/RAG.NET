using RAGNET.Application.Configuration.Queries;
using RAGNET.Application.Infrastructure.Providers.Embedding;
using RAGNET.Application.ProviderApiKeys.DTOs;
using RAGNET.Domain.ProvidersApiKeys;
using RAGNET.Domain.SharedKernel.Providers;

namespace RAGNET.Application.ProviderApiKeys.Queries.GetUserProviderApiKeys
{
    public class GetUserProviderApiKeysQueryHandler(
        IProviderApiKeyRepository providerApiKeyRepository,
        IProviderPolicyFactory providerPolicyFactory
    ) : IQueryHandler<GetUserProviderApiKeysQuery, List<ProviderApiKeyDTO>>
    {
        private readonly IProviderApiKeyRepository _providerApiKeyRepository = providerApiKeyRepository;
        private readonly IProviderPolicyFactory _providerPolicyFactory = providerPolicyFactory;
        public async Task<List<ProviderApiKeyDTO>> Handle(GetUserProviderApiKeysQuery request, CancellationToken cancellationToken)
        {
            var userApiKeys = await _providerApiKeyRepository.GetByUserIdAsync(request.User.Id);

            var result = new List<ProviderApiKeyDTO>();

            foreach (var apiKey in userApiKeys)
            {
                var provider = apiKey.Provider.ProviderType;
                var policy = _providerPolicyFactory.CreatePolicy(provider);
                apiKey.Provider.InitializePolicy(policy);
                result.Add(apiKey.ToDTO());
            }
            // Add missing providers
            // Get all supported providers
            var allProviders = Enum.GetValues<SupportedProvider>();

            foreach (var provider in allProviders)
            {
                bool alreadyExists = userApiKeys.Any(k => k.Provider.ProviderType == provider);

                if (!alreadyExists)
                {
                    var policy = _providerPolicyFactory.CreatePolicy(provider);
                    var dto = new ProviderApiKeyDTO
                    {
                        ApiKey = string.Empty,
                        ProviderId = provider,
                        Name = policy.Name,
                        Prefix = policy.Prefix,
                        Pattern = policy.Pattern,
                        Url = policy.Url
                    };

                    result.Add(dto);
                }
            }

            return result;
        }
    }
}