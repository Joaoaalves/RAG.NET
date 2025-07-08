using RAGNET.Domain.SeedWork;
using RAGNET.Domain.SharedKernel.Metas;
using RAGNET.Domain.SharedKernel.Providers.Rules;
using RAGNET.Domain.SharedKernel.VectorStorages;
using RAGNET.Infrastructure.VectorDatabases.Qdrant.Rules;

namespace RAGNET.Infrastructure.VectorDatabases.Qdrant
{
    public partial class QdrantPolicy : IVectorStoragePolicy
    {

        public string Name => "QDrant";
        public string Prefix => "";
        public string Pattern => "^\\w*.\\w*.\\w*$";
        public string Url => "https://cloud.qdrant.io/";
        public static string HostPattern => @"\b(?:https?):\/\/(?:www\.)?[a-zA-Z0-9-]+\.[a-zA-Z]{2,}(?:\/[^\s]*)?\b";
        public VectorStorageProvider ProviderType => VectorStorageProvider.QDRANT;

        public void Validate(string apiKey, IEnumerable<Meta> metas)
        {
            var regexRule = new ApiKeyMustMatchPatternRule(apiKey, Pattern);

            if (regexRule.IsBroken())
                throw new BusinessRuleValidationException(regexRule);

            var hostExistRule = new HostMustExistRule(metas);

            if (hostExistRule.IsBroken())
                throw new BusinessRuleValidationException(hostExistRule);

            var host = metas.FirstOrDefault(m => m.Key == "host")?.Value;

            var validHostRule = new HostMustBeValidRule(host!, HostPattern);
            if (validHostRule.IsBroken())
                throw new BusinessRuleValidationException(validHostRule);
        }
    }
}
