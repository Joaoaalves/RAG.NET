using RAGNET.Domain.SharedKernel.VectorStorages;

namespace RAGNET.Infrastructure.VectorDatabases.Mappers
{
    public static class ProvidersMapper
    {
        public static VectorStorageProvider ToVectorStorageProvider(this string provider)
        {
            return provider.ToLowerInvariant() switch
            {
                "pinecone" => VectorStorageProvider.PINECONE,
                "qdrant" => VectorStorageProvider.QDRANT,
                _ => throw new ArgumentOutOfRangeException("Vector Storage not supported")
            };
        }
    }
}