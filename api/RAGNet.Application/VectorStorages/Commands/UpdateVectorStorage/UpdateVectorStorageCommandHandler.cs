using RAGNET.Application.Configuration.Commands;
using RAGNET.Application.VectorStorages.DTOs;
using RAGNET.Application.VectorStorages.Mappers;
using RAGNET.Domain.SeedWork;
using RAGNET.Domain.SharedKernel.VectorStorages;
using RAGNET.Domain.Users.ApiKeys;
using RAGNET.Domain.VectorStorages;

namespace RAGNET.Application.VectorStorages.Commands.UpdateVectorStorage
{
    public class UpdateVectorStorageCommandHandler(
        IVectorStorageRepository vectorStorageRepository,
        IVectorStoragePolicyFactory vectorStoragePolicyFactory,
        IUnitOfWork unitOfWork
    ) : ICommandHandler<UpdateVectorStorageCommand, VectorStorageDTO>
    {
        private readonly IVectorStorageRepository _vectorStorageRepository = vectorStorageRepository;
        private readonly IVectorStoragePolicyFactory _vectorStoragePolicyFactory = vectorStoragePolicyFactory;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        public async Task<VectorStorageDTO> Handle(UpdateVectorStorageCommand request, CancellationToken cancellationToken)
        {
            var vectorStorage = await _vectorStorageRepository.GetByIdAsync(request.VectorStorageId, request.User.Id) ?? throw new Exception("Vector Storage was not found!");
            var provider = vectorStorage.Provider;

            if (request.ApiKey is ApiKey apiKey)
            {
                vectorStorage.UpdateApiKey(apiKey);
            }

            if (request.IsActive is bool isActive)
            {
                vectorStorage.SetIsActive(isActive);
            }

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

            vectorStorage.UpdateMetas(metas);
            var policy = _vectorStoragePolicyFactory.CreatePolicy(provider);
            policy.Validate(vectorStorage.ApiKey.Value, vectorStorage.Metas);

            await _vectorStorageRepository.UpdateAsync(vectorStorage, request.User.Id);
            await _unitOfWork.CommitAsync(cancellationToken);

            return vectorStorage.ToDTO();
        }
    }
}