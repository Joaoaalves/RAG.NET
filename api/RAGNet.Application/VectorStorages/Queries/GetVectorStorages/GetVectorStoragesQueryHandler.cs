using RAGNET.Application.Configuration.Queries;
using RAGNET.Application.VectorStorages.DTOs;
using RAGNET.Domain.SharedKernel.VectorStorages;
using RAGNET.Domain.VectorStorages;

namespace RAGNET.Application.VectorStorages.Queries.GetVectorStorages
{
    public class GetVectorStoragesQueryHandler(
        IVectorStorageRepository vectorStorageRepository,
        IVectorStoragePolicyFactory vectorStoragePolicyFactory
    ) : IQueryHandler<GetVectorStoragesQuery, List<VectorStorageApiKeyDTO>>
    {
        private readonly IVectorStorageRepository _vectorStorageRepository = vectorStorageRepository;
        private readonly IVectorStoragePolicyFactory _vectorStoragePolicyFactory = vectorStoragePolicyFactory;
        public async Task<List<VectorStorageApiKeyDTO>> Handle(GetVectorStoragesQuery request, CancellationToken cancellationToken)
        {
            List<VectorStorageApiKeyDTO> providers = [];

            foreach (var provider in Enum.GetValues<VectorStorageProvider>())
            {
                try
                {
                    var policy = _vectorStoragePolicyFactory.CreatePolicy(provider);
                    var storage = await _vectorStorageRepository.GetByUserIdAndProviderAsync(request.User.Id, provider);

                    var providerDto = new VectorStorageApiKeyDTO
                    {
                        Id = storage?.Id.Value ?? Guid.Empty,
                        ProviderId = provider,
                        ApiKey = storage?.ApiKey.Value ?? string.Empty,
                        Name = provider.ToString(),
                        Pattern = policy.Pattern,
                        Prefix = policy.Prefix,
                        Url = policy.Url,
                        Schema = policy.Schema
                    };

                    providers.Add(providerDto);
                }
                catch (Exception exc)
                {
                    Console.WriteLine(exc.Message);
                }
            }
            return providers;
        }
    }
}