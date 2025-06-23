using RAGNET.Application.Configuration.Queries;

namespace RAGNET.Application.ProviderApiKeys.GetUserProviderApiKeys
{
    public class GetUserProviderApiKeysQuery(
    ) : UserAwareQuery<List<ProviderApiKeyDTO>>
    {
    }
}