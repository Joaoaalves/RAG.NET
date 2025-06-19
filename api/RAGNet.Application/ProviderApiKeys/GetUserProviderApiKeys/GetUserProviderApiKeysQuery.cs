using RAGNET.Application.Configuration.Queries;

namespace RAGNET.Application.ProviderApiKeys.GetUserProviderApiKeys
{
    public class GetUserProviderApiKeysQuery(
        string userId
    ) : IQuery<List<ProviderApiKeyDTO>>
    {
        public string UserId { get; } = userId;
    }
}