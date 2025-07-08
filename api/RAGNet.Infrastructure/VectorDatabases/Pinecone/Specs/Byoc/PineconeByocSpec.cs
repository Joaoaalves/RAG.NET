using RAGNET.Domain.SharedKernel.Metas;
using RAGNET.Domain.SharedKernel.VectorStorages;
using RAGNET.Domain.VectorStorages;

namespace RAGNET.Infrastructure.VectorDatabases.Pinecone.Specs.Byoc
{
    public sealed class PineconeByocSpec : IVectorStorageSpec
    {
        public VectorStorageProvider Provider => VectorStorageProvider.PINECONE;
        public static PineconeIndexType IndexType { get; } = PineconeIndexType.Byoc;

        public string Environment { get; init; } = string.Empty;

        public Dictionary<string, string> ToMeta() => new()
        {
            ["indexType"] = IndexType.ToString(),
            ["environment"] = Environment
        };

        public static PineconeByocSpec FromMeta(IEnumerable<Meta> metas)
        {
            try
            {
                var environment = metas.FirstOrDefault(m => m.Key == "environment")?.Value ?? throw new ArgumentNullException("Environment");
                var apiKeyString = metas.FirstOrDefault(m => m.Key == "apiKey")?.Value ?? throw new ArgumentNullException("Api Key");

                return new()
                {
                    Environment = environment,
                };
            }
            catch (ArgumentNullException exc)
            {
                throw new ArgumentNullException($"Pinecone Byoc Spec - {exc.Message} not found.");
            }
        }
    }
}