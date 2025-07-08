using RAGNET.Application.Configuration.Queries;
using RAGNET.Application.VectorStorages.DTOs;

namespace RAGNET.Application.VectorStorages.Queries.GetUserVectorStorages
{
    public class GetUserVectorStoragesQuery : UserAwareQuery<List<VectorStorageDTO>>
    {
    }
}