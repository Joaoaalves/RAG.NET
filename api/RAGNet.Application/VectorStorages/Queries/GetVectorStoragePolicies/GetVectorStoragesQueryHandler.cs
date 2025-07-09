using RAGNET.Application.Configuration.Queries;
using RAGNET.Application.VectorStorages.DTOs;
using RAGNET.Domain.SharedKernel.VectorStorages;
using RAGNET.Domain.VectorStorages;

namespace RAGNET.Application.VectorStorages.Queries.GetVectorStoragePolicies
{
    public class GetVectorStoragePoliciesQueryHandler(
        IVectorStorageRepository vectorStorageRepository,
        IVectorStoragePolicyFactory vectorStoragePolicyFactory
    ) : IQueryHandler<GetVectorStoragePoliciesQuery, List<VectorStoragePolicyDTO>>
    {
        private readonly IVectorStorageRepository _vectorStorageRepository = vectorStorageRepository;
        private readonly IVectorStoragePolicyFactory _vectorStoragePolicyFactory = vectorStoragePolicyFactory;
        public async Task<List<VectorStoragePolicyDTO>> Handle(GetVectorStoragePoliciesQuery request, CancellationToken cancellationToken)
        {
            List<VectorStoragePolicyDTO> vsPolcies = [];

            foreach (var provider in Enum.GetValues<VectorStorageProvider>())
            {
                try
                {
                    var policy = _vectorStoragePolicyFactory.CreatePolicy(provider);
                    var storage = await _vectorStorageRepository.GetByUserIdAndProviderAsync(request.User.Id, provider);

                    var providerDto = new VectorStoragePolicyDTO
                    {
                        Id = storage?.Id.Value ?? Guid.Empty,
                        ProviderId = provider,
                        ApiKey = storage?.ApiKey.Value ?? string.Empty,
                        Name = provider.ToString(),
                        Pattern = policy.Pattern,
                        Prefix = policy.Prefix,
                        Url = policy.Url
                    };

                    vsPolcies.Add(providerDto);
                }
                catch (Exception exc)
                {
                    Console.WriteLine(exc.Message);
                }
            }
            return vsPolcies;
        }
    }
}