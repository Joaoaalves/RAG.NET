using Microsoft.Extensions.Options;
using Pinecone;
using RAGNET.Domain.SeedWork;
using RAGNET.Domain.SharedKernel.Metas;
using RAGNET.Domain.SharedKernel.Providers.Rules;
using RAGNET.Domain.SharedKernel.VectorStorages;
using RAGNET.Infrastructure.VectorDatabases.Pinecone.Rules;

namespace RAGNET.Infrastructure.VectorDatabases.Pinecone
{
    public partial class PineconePolicy : IVectorStoragePolicy
    {

        public string Name => "Pinecone";
        public string Prefix => "pcsk_";
        public string Pattern => @"^pcsk_\w+(_\w+)*$";
        public string Url => "https://app.pinecone.io/organizations/-/projects";
        public VectorStorageProvider ProviderType => VectorStorageProvider.PINECONE;

        public void Validate(string apiKey, IEnumerable<Meta> metas)
        {
            var regexRule = new ApiKeyMustMatchPatternRule(apiKey, Pattern);

            if (regexRule.IsBroken())
                throw new BusinessRuleValidationException(regexRule);

            var indexTypeStr = metas.FirstOrDefault(m => m.Key == "indexType")?.Value;
            var indexTypeMustBeValid = new IndexTypeMustBeValidRule(indexTypeStr!);

            if (indexTypeMustBeValid.IsBroken())
                throw new BusinessRuleValidationException(indexTypeMustBeValid);

            Enum.TryParse(indexTypeStr, out PineconeIndexType indexType);

            if (indexType == PineconeIndexType.Serverless)
                ValidateServerless(metas);

            if (indexType == PineconeIndexType.Pod)
                ValidatePod(metas);

            if (indexType == PineconeIndexType.Byoc)
                ValidateByoc(metas);
        }

        private static void ValidateByoc(IEnumerable<Meta> metas)
        {
            var environmentStr = metas.FirstOrDefault(m => m.Key == "environment")?.Value;
            var environmentMustBeValidRule = new EnvironmentMustBeValidRule(environmentStr);
            if (environmentMustBeValidRule.IsBroken())
                throw new BusinessRuleValidationException(environmentMustBeValidRule);
        }

        private static void ValidatePod(IEnumerable<Meta> metas)
        {
            var podSizeStr = metas.FirstOrDefault(m => m.Key == "podSize")?.Value;
            var podSizeMustBeValidRule = new PodSizeMustBeValidRule(podSizeStr);
            if (podSizeMustBeValidRule.IsBroken())
                throw new BusinessRuleValidationException(podSizeMustBeValidRule);

            var podTypeStr = metas.FirstOrDefault(m => m.Key == "podType")?.Value;
            var podTypeMustBeValidRule = new PodTypeMustBeValidRule(podTypeStr);
            if (podTypeMustBeValidRule.IsBroken())
                throw new BusinessRuleValidationException(podTypeMustBeValidRule);

            var podsStr = metas.FirstOrDefault(m => m.Key == "pods")?.Value;
            var podsMustBeValidRule = new PodsMustBeValidRule(podsStr);
            if (podsMustBeValidRule.IsBroken())
                throw new BusinessRuleValidationException(podsMustBeValidRule);

            var environmentStr = metas.FirstOrDefault(m => m.Key == "environment")?.Value;
            var environmentMustBeValidRule = new EnvironmentMustBeValidRule(environmentStr);
            if (environmentMustBeValidRule.IsBroken())
                throw new BusinessRuleValidationException(environmentMustBeValidRule);
        }

        private static void ValidateServerless(IEnumerable<Meta> metas)
        {
            var cloudStr = metas.FirstOrDefault(m => m.Key == "cloud")?.Value;
            var cloudMustBeValidRule = new CloudMustBeValidRule(cloudStr);
            if (cloudMustBeValidRule.IsBroken())
                throw new BusinessRuleValidationException(cloudMustBeValidRule);

            Enum.TryParse(cloudStr, out ServerlessSpecCloud cloud);

            var regionStr = metas.FirstOrDefault(m => m.Key == "region")?.Value;
            var regionMustBeValid = new RegionMustBeValidRule(regionStr, cloud);
            if (regionMustBeValid.IsBroken())
                throw new BusinessRuleValidationException(regionMustBeValid);
        }
    }
}