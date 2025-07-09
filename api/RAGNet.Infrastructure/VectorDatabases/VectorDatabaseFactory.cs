using Pinecone;
using Qdrant.Client;

using RAGNET.Domain.SharedKernel.VectorStorages;

using RAGNET.Application.Infrastructure.Providers;
using RAGNET.Application.Infrastructure.Providers.VectorDatabases;

using RAGNET.Infrastructure.VectorDatabases.Pinecone;
using RAGNET.Infrastructure.VectorDatabases.Qdrant;
using RAGNET.Domain.VectorStorages;
using RAGNET.Infrastructure.VectorDatabases.Qdrant.Specs;
using RAGNET.Infrastructure.VectorDatabases.Pinecone.Specs.Serverless;
using RAGNET.Infrastructure.VectorDatabases.Pinecone.Specs.Pod;
using RAGNET.Infrastructure.VectorDatabases.Pinecone.Specs.Byoc;

namespace RAGNET.Infrastructure.VectorDatabases
{
    public class VectorDatabaseFactory(
        IVectorStorageRepository vectorStorageRepository
    ) : IVectorDatabaseFactory
    {
        private readonly IVectorStorageRepository _vectorStorageRepository = vectorStorageRepository;
        public async Task<IVectorDatabaseService> CreateVectorDatabaseServiceAsync(VectorStorageId vectorStorageId, string userId)
        {
            var vectorStorage = await _vectorStorageRepository.GetByIdAsync(vectorStorageId, userId) ?? throw new Exception("Invalid Vector Storage");
            return vectorStorage.Provider switch
            {
                VectorStorageProvider.QDRANT => QdrantClient(vectorStorage),
                VectorStorageProvider.PINECONE => PineconeClient(vectorStorage),
                _ => throw new NotSupportedException("Vector Database not supported")
            };
        }

        private static QDrantAdapter QdrantClient(VectorStorage vectorStorage)
        {
            var qdrantSpec = QdrantSpec.FromMeta(vectorStorage.Metas);

            var host = qdrantSpec.Host;
            if (host is not null)
            {
                var client = new QdrantClient(host.Value, https: true, apiKey: vectorStorage.ApiKey.Value);
                return new QDrantAdapter(client);
            }

            throw new ArgumentNullException("Host was not provided for QDrant.");
        }

        private static PineconeAdapter PineconeClient(VectorStorage vectorStorage)
        {
            var indexTypeString = vectorStorage.Metas.FirstOrDefault(meta => meta.Key == "indexType")?.Value ?? throw new Exception("Pinecone index type not found");
            if (Enum.TryParse(indexTypeString, out PineconeIndexType indexType))
            {
                var client = new PineconeClient(vectorStorage.ApiKey.Value);

                if (indexType == PineconeIndexType.Serverless)
                {
                    var spec = PineconeServerlessInfraSpec.FromMeta(vectorStorage.Metas);
                    return PineconeAdapter.Serverless(client, spec.Cloud, spec.Region);
                }

                if (indexType == PineconeIndexType.Pod)
                {
                    var spec = PineconePodInfraSpec.FromMeta(vectorStorage.Metas);

                    return PineconeAdapter.Pod(client, spec.PodType, spec.PodSize, spec.Pods, spec.Environment);
                }

                if (indexType == PineconeIndexType.Byoc)
                {
                    Console.WriteLine(vectorStorage.ApiKey.Value);
                    var spec = PineconeByocSpec.FromMeta(vectorStorage.Metas);

                    return PineconeAdapter.Byoc(client, spec.Environment);
                }

            }

            throw new Exception("Cant build Pinecone Client");
        }
    }
}