using RAGNET.Domain.SeedWork;

namespace RAGNET.Domain.ProvidersApiKeys
{
    public class ProviderApiKeyId : TypedIdValueBase
    {
        public ProviderApiKeyId(Guid value) : base(value)
        {
        }

        public ProviderApiKeyId() : base(Guid.NewGuid())
        {
        }
    }
}