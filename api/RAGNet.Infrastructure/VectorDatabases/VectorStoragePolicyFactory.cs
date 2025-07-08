using RAGNET.Domain.SharedKernel.VectorStorages;
using RAGNET.Infrastructure.VectorDatabases.Pinecone;
using RAGNET.Infrastructure.VectorDatabases.Qdrant;

namespace RAGNET.Infrastructure.VectorDatabases
{
    public class VectorStoragePolicyFactory(
        SchemaPathsOptions options
    ) : IVectorStoragePolicyFactory
    {
        private readonly SchemaPathsOptions _options = options;
        public IVectorStoragePolicy CreatePolicy(VectorStorageProvider provider)
        {
            return provider switch
            {
                VectorStorageProvider.PINECONE => CreatePineconePolicy(),
                VectorStorageProvider.QDRANT => CreateQdrandPolicy(),
                _ => throw new ArgumentOutOfRangeException(nameof(provider), "Unsupported vector storage")
            };
        }

        private PineconePolicy CreatePineconePolicy()
        {

            var fullPath = Path.GetFullPath(_options.Pinecone);
            if (!File.Exists(fullPath))
                throw new FileNotFoundException("pinecone-schema.json not found at", fullPath);

            var schema = File.ReadAllText(fullPath);

            return new PineconePolicy(schema);
        }

        private QdrantPolicy CreateQdrandPolicy()
        {
            var fullPath = Path.GetFullPath(_options.Qdrant);
            if (!File.Exists(fullPath))
                throw new FileNotFoundException("qdrant-schema.json not found at", fullPath);

            var schema = File.ReadAllText(fullPath);

            return new QdrantPolicy(schema);
        }

    }
}