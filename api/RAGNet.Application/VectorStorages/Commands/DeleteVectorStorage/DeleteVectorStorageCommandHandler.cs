using RAGNET.Application.Configuration.Commands;
using RAGNET.Domain.SeedWork;
using RAGNET.Domain.VectorStorages;

namespace RAGNET.Application.VectorStorages.Commands.DeleteVectorStorage
{
    public class DeleteVectorStorageCommandHandler(
        IVectorStorageRepository vectorStorageRepository,
        IUnitOfWork unitOfWork
    ) : ICommandHandler<DeleteVectorStorageCommand, Unit>
    {
        private readonly IVectorStorageRepository _vectorStorageRepository = vectorStorageRepository;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        public async Task<Unit> Handle(DeleteVectorStorageCommand request, CancellationToken cancellationToken)
        {
            var vectorStorage = await _vectorStorageRepository.GetByIdAsync(request.VectorStorageId, request.User.Id) ?? throw new Exception("Vector Storage not found.");
            await _vectorStorageRepository.DeleteAsync(vectorStorage, request.User.Id);
            await _unitOfWork.CommitAsync(cancellationToken);
            return Unit.Value;
        }
    }
}