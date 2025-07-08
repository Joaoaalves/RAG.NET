using RAGNET.Application.Configuration.Queries;
using RAGNET.Application.VectorStorages.DTOs;

namespace RAGNET.Application.VectorStorages.Queries.GetVectorStorages
{
    public class GetVectorStoragesQuery : UserAwareQuery<List<VectorStorageApiKeyDTO>>
    {
    }
}