using Qdrant.Client;
using Qdrant.Client.Grpc;

using RAGNET.Application.Infrastructure.Providers;
using RAGNET.Application.Infrastructure.Providers.Embedding.DTOs;
using RAGNET.Domain.Documents.Pages.Chunks;

namespace RAGNET.Infrastructure.Qdrant
{
    /// <summary>
    /// Adapter that wraps Qdrant's vector database client and exposes domain-friendly operations.
    /// </summary>
    public class QDrantAdapter : IVectorDatabaseService
    {
        private readonly QdrantClient _client;

        public QDrantAdapter()
        {
            // Initialize the Qdrant client, assuming it connects to a container or service named "qdrant"
            _client = new QdrantClient("qdrant");
        }

        /// <summary>
        /// Creates a new vector collection in Qdrant with the specified vector size.
        /// </summary>
        public async Task CreateCollectionAsync(Guid collectionName, int vectorSize)
        {
            await _client.CreateCollectionAsync(collectionName.ToString(), new VectorParams
            {
                Size = (ulong)vectorSize,
                Distance = Distance.Cosine  // Cosine distance is typically used for embeddings
            });
        }

        /// <summary>
        /// Inserts a single embedding into the specified collection.
        /// </summary>
        public async Task InsertAsync(EmbeddingDTO embedding, string collectionName)
        {
            try
            {
                // Add required metadata to the payload
                embedding.Metadata["documentId"] = embedding.VectorId;

                var vectorArray = embedding.Vector.ToArray(); // Convert SemanticVector to float[]

                var point = new PointStruct
                {
                    Id = Guid.NewGuid(),
                    Vectors = vectorArray,
                    Payload = {
                        embedding.Metadata.ToDictionary(
                            kvp => kvp.Key,
                            kvp => new Value { StringValue = kvp.Value }
                        )
                    }
                };

                await _client.UpsertAsync(collectionName, [point]);
            }
            catch (Exception ex)
            {
                // Always wrap and rethrow with context so higher-level services can handle/log properly
                throw new InvalidOperationException(
                    $"Error inserting vector into collection '{collectionName}': {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Inserts multiple embeddings in a batch into the specified collection.
        /// </summary>
        public async Task InsertManyAsync(IEnumerable<EmbeddingDTO> batch, string collectionName)
        {
            try
            {
                var points = batch.Select(entry =>
                {
                    var metadataWithId = new Dictionary<string, string>(entry.Metadata)
                    {
                        ["documentId"] = entry.VectorId,
                        ["vectorId"] = entry.VectorId
                    };

                    var vectorArray = entry.Vector.ToArray();

                    return new PointStruct
                    {
                        Id = Guid.NewGuid(),
                        Vectors = vectorArray,
                        Payload = {
                            metadataWithId.ToDictionary(
                                kvp => kvp.Key,
                                kvp => new Value { StringValue = kvp.Value }
                            )
                        }
                    };
                });

                await _client.UpsertAsync(collectionName, [.. points]);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException(
                    $"Error inserting batch into collection '{collectionName}': {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Performs a nearest-neighbor search using a single query vector.
        /// </summary>
        public async Task<List<VectorQueryResult>> QueryAsync(SemanticVector vector, string collectionName, int topK)
        {
            var searchParams = new SearchParams
            {
                Exact = false,
                HnswEf = 128  // Search performance parameter for HNSW (Higher = more accurate)
            };

            var vectorArray = vector.ToArray();

            var response = await _client.QueryAsync(
                collectionName: collectionName,
                query: vectorArray,
                filter: null,
                searchParams: searchParams,
                limit: (ulong)topK
            );

            // Parse Qdrant results into domain DTOs
            var results = response.Select(point =>
            {
                string vectorId = string.Empty;

                // Ensure we only read string values from the payload
                if (point.Payload.TryGetValue("vectorId", out var val) &&
                    val.KindCase == Value.KindOneofCase.StringValue)
                {
                    vectorId = val.StringValue;
                }

                return new VectorQueryResult
                {
                    VectorId = vectorId,
                    Score = point.Score
                };
            }).ToList();

            return results;
        }

        /// <summary>
        /// Queries multiple vectors in parallel and flattens all the results into a single list.
        /// </summary>
        public async Task<List<VectorQueryResult>> QueryMultipleAsync(IEnumerable<SemanticVector> vectors, string collectionName, int topK)
        {
            // Each query executes concurrently to improve throughput
            var tasks = vectors.Select(vector =>
                QueryAsync(vector, collectionName, topK));

            var results = await Task.WhenAll(tasks);

            // Flatten the individual result lists into one
            return results.SelectMany(r => r).ToList();
        }

        /// <summary>
        /// Combines multiple vectors into their average and performs a single query.
        /// This is useful for hybrid or multi-part queries.
        /// </summary>
        public async Task<List<VectorQueryResult>> QueryByAverageVectorAsync(IEnumerable<SemanticVector> queryVectors, string collectionName, int topK)
        {
            var vectorList = queryVectors.ToList();
            if (vectorList.Count == 0)
                throw new ArgumentException("At least one query vector is required.", nameof(queryVectors));

            int vectorSize = vectorList.First().Size();
            float[] combinedVector = new float[vectorSize];

            // Add all vector components element-wise
            foreach (var vector in vectorList)
            {
                for (int i = 0; i < vectorSize; i++)
                {
                    combinedVector[i] += vector.At(i);
                }
            }

            // Divide by number of vectors to compute average
            for (int i = 0; i < vectorSize; i++)
            {
                combinedVector[i] /= vectorList.Count;
            }

            var averageVector = new SemanticVector(combinedVector);

            // Perform the query using the averaged vector
            return await QueryAsync(averageVector, collectionName, topK);
        }
    }
}
