using RAGNET.Application.Configuration.Queries;
using RAGNET.Application.ProviderApiKeys.DTOs;

namespace RAGNET.Application.ProviderApiKeys.Queries.GetUserProviderApiKeys
{
    public class GetUserProviderApiKeysQuery(
    ) : UserAwareQuery<List<ProviderApiKeyDTO>>
    {
    }
}