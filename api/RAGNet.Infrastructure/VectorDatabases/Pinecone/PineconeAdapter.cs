using Pinecone;

using RAGNET.Domain.SharedKernel.VectorStorages.SemanticVectors;

using RAGNET.Application.Infrastructure.Providers;
using RAGNET.Application.Infrastructure.Providers.Embedding.DTOs;
using RAGNET.Infrastructure.VectorDatabases.Pinecone.Specs.Pod;

namespace RAGNET.Infrastructure.VectorDatabases.Pinecone
{
    public class PineconeAdapter : IVectorDatabaseService
    {
        private readonly PineconeClient Client = null!;
        private readonly PineconeIndexType IndexType;
        private readonly PodSizes? PodSize;
        private readonly PodTypes? PodType;
        private readonly uint? Pods;
        private readonly string? Environment;
        private readonly ServerlessSpecCloud? Cloud;
        private readonly string? Region;
        private PineconeAdapter(
            PineconeClient client,
            PineconeIndexType indexType,
            PodSizes? podSize = null,
            PodTypes? podType = null,
            uint? pods = null,
            string? environment = null,
            ServerlessSpecCloud? cloud = null,
            string? region = null)
        {
            Client = client;
            IndexType = indexType;
            PodSize = podSize;
            PodType = podType;
            Pods = pods;
            Environment = environment;
            Cloud = cloud;
            Region = region;
        }
        public static PineconeAdapter Serverless(PineconeClient client, ServerlessSpecCloud cloud, string region)
        {
            return new PineconeAdapter(
                client,
                PineconeIndexType.Serverless,
                cloud: cloud,
                region: region
            );
        }

        public static PineconeAdapter Pod(PineconeClient client, PodTypes podType, PodSizes podSize, uint pods, string environment)
        {
            return new PineconeAdapter(
                client,
                PineconeIndexType.Pod,
                podType: podType,
                podSize: podSize,
                pods: pods,
                environment: environment
            );
        }

        public static PineconeAdapter Byoc(PineconeClient client, string environment)
        {
            return new PineconeAdapter(
                client,
                PineconeIndexType.Byoc,
                environment: environment
            );
        }

        public async Task CreateCollectionAsync(Guid collectionName, int vectorSize)
        {
            var indexName = collectionName.ToString();
            await CreateIndex(indexName, vectorSize);

            var collectionRequest = new CreateCollectionRequest
            {
                Name = indexName,
                Source = indexName
            };

            await Client.CreateCollectionAsync(collectionRequest);
        }

        public async Task InsertAsync(EmbeddingDTO embedding, string collectionName)
        {
            var index = Client.Index(collectionName);

            embedding.Metadata["documentId"] = embedding.VectorId;

            var vector = new Vector
            {
                Id = embedding.VectorId,
                Values = embedding.Vector.ToArray(),
                Metadata = new Metadata(
                    embedding.Metadata.Select(kvp =>
                        new KeyValuePair<string, MetadataValue?>(kvp.Key, new MetadataValue(kvp.Value))
                    )
                )
            };

            var request = new UpsertRequest
            {
                Vectors = [vector]
            };

            await index.UpsertAsync(request);
        }

        public async Task InsertManyAsync(IEnumerable<EmbeddingDTO> batch, string collectionName)
        {
            var index = Client.Index(collectionName);

            var vectors = batch.Select(embedding =>
            {
                var metadataWithId = new Dictionary<string, string>(embedding.Metadata)
                {
                    ["documentId"] = embedding.VectorId,
                    ["vectorId"] = embedding.VectorId
                };

                return new Vector
                {
                    Id = embedding.VectorId,
                    Values = embedding.Vector.ToArray(),
                    Metadata = new Metadata(
                        metadataWithId.Select(kvp =>
                            new KeyValuePair<string, MetadataValue?>(kvp.Key, new MetadataValue(kvp.Value))
                        )
                    )
                };
            });

            var request = new UpsertRequest
            {
                Vectors = vectors.ToArray()
            };

            await index.UpsertAsync(request);
        }

        public async Task<List<VectorQueryResult>> QueryAsync(SemanticVector vector, string collectionName, int topK)
        {
            var index = Client.Index(collectionName);

            var request = new QueryRequest
            {
                Vector = vector.ToArray(),
                TopK = (uint)topK,
                IncludeMetadata = true
            };

            var result = await index.QueryAsync(request);
            if (result.Matches is not null)
            {
                return result.Matches.Select(match =>
                {
                    string vectorId = match.Id;
                    string? extractedId = null;

                    if (match.Metadata?.TryGetValue("vectorId", out var value) == true && value is not null)
                        extractedId = value.Value.ToString();

                    return new VectorQueryResult
                    {
                        VectorId = extractedId ?? vectorId,
                        Score = match.Score ?? 0f
                    };
                }).ToList();
            }

            return [];
        }

        public async Task<List<VectorQueryResult>> QueryMultipleAsync(IEnumerable<SemanticVector> vectors, string collectionName, int topK)
        {
            var tasks = vectors.Select(vector =>
                QueryAsync(vector, collectionName, topK));

            var results = await Task.WhenAll(tasks);

            return results.SelectMany(r => r).ToList();
        }

        public async Task<List<VectorQueryResult>> QueryByAverageVectorAsync(IEnumerable<SemanticVector> queryVectors, string collectionName, int topK)
        {
            var vectorList = queryVectors.ToList();
            if (vectorList.Count == 0)
                throw new ArgumentException("At least one query vector is required.", nameof(queryVectors));

            int vectorSize = vectorList.First().Size();
            float[] combinedVector = new float[vectorSize];

            foreach (var vector in vectorList)
            {
                for (int i = 0; i < vectorSize; i++)
                    combinedVector[i] += vector.At(i);
            }

            for (int i = 0; i < vectorSize; i++)
                combinedVector[i] /= vectorList.Count;

            var averageVector = new SemanticVector(combinedVector);

            return await QueryAsync(averageVector, collectionName, topK);
        }

        private async Task CreateIndex(string indexName, int dimension)
        {
            var indexRequest = new CreateIndexRequest
            {
                Name = indexName,
                Dimension = dimension,
                Spec = CreateIndexSpec(),
                Metric = MetricType.Cosine,
            };

            await Client.CreateIndexAsync(indexRequest);
        }

        private OneOf.OneOf<ServerlessIndexSpec, PodIndexSpec, ByocIndexSpec> CreateIndexSpec()
        {
            if (IndexType == PineconeIndexType.Serverless && Cloud is not null && Region is not null)
            {
                return new ServerlessIndexSpec
                {
                    Serverless = new ServerlessSpec
                    {
                        Cloud = (ServerlessSpecCloud)Cloud,
                        Region = Region
                    }
                };
            }

            if (IndexType == PineconeIndexType.Pod)
            {
                if (Environment is not null &&
                    PodType is not null &&
                    PodSize is not null &&
                    Pods is not null
                )
                {
                    return new PodIndexSpec
                    {
                        Pod = new PodSpec
                        {
                            Environment = Environment,
                            PodType = $"{PodType.ToString()!.ToLowerInvariant()}.{PodSize.ToString()!.ToLowerInvariant()}",
                            Pods = (int)Pods
                        }
                    };
                }
            }

            if (IndexType == PineconeIndexType.Byoc && Environment is not null)
            {
                return new ByocIndexSpec
                {
                    Byoc = new ByocSpec
                    {
                        Environment = Environment
                    }
                };
            }

            throw new Exception("Cant build Index Spec");
        }
    }
}
