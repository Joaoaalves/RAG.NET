using Pinecone;
using RAGNET.Domain.SharedKernel.Metas;
using RAGNET.Domain.SharedKernel.VectorStorages;
using RAGNET.Domain.VectorStorages;

namespace RAGNET.Infrastructure.VectorDatabases.Pinecone.Specs.Serverless
{
    public sealed class PineconeServerlessInfraSpec : IVectorStorageSpec
    {
        public VectorStorageProvider Provider => VectorStorageProvider.PINECONE;
        public PineconeIndexType IndexType { get; } = PineconeIndexType.Serverless;
        public ServerlessSpecCloud Cloud { get; init; }
        public string Region { get; init; } = string.Empty;
        public Dictionary<string, string> ToMeta() => new()
        {
            ["cloud"] = Cloud.ToString(),
            ["indexType"] = IndexType.ToString(),
            ["region"] = Region
        };

        public static PineconeServerlessInfraSpec FromMeta(IEnumerable<Meta> metas)
        {
            try
            {
                var cloud = metas.FirstOrDefault(m => m.Key == "cloud")?.Value ?? throw new ArgumentNullException("Cloud");
                ServerlessSpecCloud validCloud = cloud.ToServerlessSpecCloud();

                var region = metas.FirstOrDefault(m => m.Key == "region")?.Value ?? throw new ArgumentNullException("Region"); ;
                string validRegion = region.ToServerlessRegion(validCloud);
                var apiKeyString = metas.FirstOrDefault(m => m.Key == "apiKey")?.Value ?? throw new ArgumentNullException("Api Key");

                return new()
                {
                    Cloud = validCloud,
                    Region = validRegion,
                };
            }
            catch (ArgumentNullException exc)
            {
                throw new ArgumentNullException($"Pinecone Serverless Spec - {exc.Message} not found.");
            }
        }
    }
}