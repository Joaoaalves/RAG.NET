using RAGNET.Application.Configuration.Queries;
using RAGNET.Application.VectorStorages.DTOs;

namespace RAGNET.Application.VectorStorages.Queries.GetVectorStoragePolicies
{
    public class GetVectorStoragePoliciesQuery : UserAwareQuery<List<VectorStoragePolicyDTO>>
    {
    }
}