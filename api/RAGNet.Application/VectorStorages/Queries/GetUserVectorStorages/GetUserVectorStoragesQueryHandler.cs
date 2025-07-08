using RAGNET.Application.Configuration.Queries;
using RAGNET.Application.VectorStorages.DTOs;
using RAGNET.Domain.VectorStorages;

namespace RAGNET.Application.VectorStorages.Queries.GetUserVectorStorages
{
    public class GetUserVectorStoragesQueryHandler(
        IVectorStorageRepository vectorStorageRepository
    ) : IQueryHandler<GetUserVectorStoragesQuery, List<VectorStorageDTO>>
    {
        private readonly IVectorStorageRepository _vectorStorageRepository = vectorStorageRepository;
        public async Task<List<VectorStorageDTO>> Handle(GetUserVectorStoragesQuery request, CancellationToken cancellationToken)
        {
            var vectorStorages = await _vectorStorageRepository.GetByUserIdAsync(request.User.Id);

            List<VectorStorageDTO> providers = [];

            foreach (var storage in vectorStorages)
            {
                try
                {
                    var providerDto = new VectorStorageDTO
                    {
                        Id = storage.Id.Value,
                        Provider = storage.Provider,
                        Name = storage.Provider.ToString()
                    };

                    providers.Add(providerDto);
                }
                catch (Exception)
                { }
            }
            return providers;
        }
    }
}