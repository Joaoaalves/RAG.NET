using RAGNET.Application.Configuration.Commands;
using RAGNET.Domain.VectorStorages;

namespace RAGNET.Application.VectorStorages.Commands.DeleteVectorStorage
{
    public class DeleteVectorStorageCommand(
        VectorStorageId id
    ) : UserAwareCommand<Unit>
    {
        public VectorStorageId VectorStorageId { get; set; } = id;
    }
}