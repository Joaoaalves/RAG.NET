using RAGNET.Application.Configuration.Commands;
using RAGNET.Application.VectorStorages.DTOs;
using RAGNET.Application.VectorStorages.Mappers;
using RAGNET.Domain.SeedWork;
using RAGNET.Domain.SharedKernel.VectorStorages;
using RAGNET.Domain.VectorStorages;

namespace RAGNET.Application.VectorStorages.Commands.AddVectorStorage
{
    public class AddVectorStorageCommandHandler(
        IVectorStorageRepository vectorStorageRepository,
        IVectorStoragePolicyFactory vectorStoragePolicyFactory,
        IUnitOfWork unitOfWork
    ) : ICommandHandler<AddVectorStorageCommand, VectorStorageDTO>
    {
        private readonly IVectorStorageRepository _vectorStorageRepository = vectorStorageRepository;
        private readonly IVectorStoragePolicyFactory _vectorStoragePolicyFactory = vectorStoragePolicyFactory;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        public async Task<VectorStorageDTO> Handle(AddVectorStorageCommand request, CancellationToken cancellationToken)
        {
            var user = request.User;
            var provider = request.Provider;
            var apiKey = request.ApiKey;


            var storage = VectorStorage.Create(
                provider,
                user.Id,
                apiKey
            );
            Dictionary<string, string> metas = [];

            if (provider == VectorStorageProvider.QDRANT)
            {
                metas.Add("host", request.Host!);
            }
            if (provider == VectorStorageProvider.PINECONE)
            {
                metas.Add("indexType", request.IndexType.ToString()!);
                metas.Add("pods", request.Pods.ToString()!);
                metas.Add("podSize", request.PodSize!);
                metas.Add("podType", request.PodType!);
                metas.Add("environment", request.Environment!);
                metas.Add("cloud", request.Cloud.ToString()!);
                metas.Add("region", request.Region!);
            }

            storage.SetSpecMetas(metas!);
            var policy = _vectorStoragePolicyFactory.CreatePolicy(provider);
            policy.Validate(apiKey.Value, storage.Metas);

            await _vectorStorageRepository.AddAsync(storage);
            await _unitOfWork.CommitAsync(cancellationToken);

            return storage.ToDTO();
        }
    }
}