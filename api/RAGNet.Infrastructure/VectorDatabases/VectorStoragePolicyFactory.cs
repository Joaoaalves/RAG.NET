using RAGNET.Domain.SharedKernel.VectorStorages;
using RAGNET.Infrastructure.VectorDatabases.Pinecone;
using RAGNET.Infrastructure.VectorDatabases.Qdrant;

namespace RAGNET.Infrastructure.VectorDatabases
{
    public class VectorStoragePolicyFactory
    : IVectorStoragePolicyFactory
    {
        public IVectorStoragePolicy CreatePolicy(VectorStorageProvider provider)
        {
            return provider switch
            {
                VectorStorageProvider.PINECONE => CreatePineconePolicy(),
                VectorStorageProvider.QDRANT => CreateQdrandPolicy(),
                _ => throw new ArgumentOutOfRangeException(nameof(provider), "Unsupported vector storage")
            };
        }

        private static PineconePolicy CreatePineconePolicy()
        {
            return new PineconePolicy();
        }

        private static QdrantPolicy CreateQdrandPolicy()
        {
            return new QdrantPolicy();
        }

    }
}