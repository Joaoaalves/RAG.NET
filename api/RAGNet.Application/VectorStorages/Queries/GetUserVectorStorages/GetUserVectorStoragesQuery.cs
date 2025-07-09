using RAGNET.Application.Configuration.Queries;
using RAGNET.Application.VectorStorages.DTOs;
using RAGNET.Domain.SharedKernel.VectorStorages;

namespace RAGNET.Application.VectorStorages.Queries.GetUserVectorStorages
{
    public class GetUserVectorStoragesQuery : UserAwareQuery<List<VectorStorageDTO>>
    {
    }
}