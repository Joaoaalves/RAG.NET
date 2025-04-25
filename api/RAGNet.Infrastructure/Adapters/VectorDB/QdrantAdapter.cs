using Qdrant.Client;
using Qdrant.Client.Grpc;
using RAGNET.Domain.Services;

namespace RAGNET.Infrastructure.Adapters.VectorDB
{
    public class QDrantAdapter : IVectorDatabaseService
    {
        private readonly QdrantClient _client;

        public QDrantAdapter()
        {
            _client = new QdrantClient("qdrant");
        }

        public async Task CreateCollectionAsync(Guid collectionName, int vectorSize)
        {
            await _client.CreateCollectionAsync(collectionName.ToString(), new VectorParams
            {
                Size = (ulong)vectorSize,
                Distance = Distance.Cosine
            });
        }

        public async Task InsertAsync(string documentId, float[] vector, string collectionName, Dictionary<string, string> metadata)
        {
            try
            {
                metadata["documentId"] = documentId;

                var point = new PointStruct
                {
                    Id = Guid.NewGuid(),
                    Vectors = vector,
                    Payload = {
                        metadata.ToDictionary(
                            kvp => kvp.Key,
                            kvp => new Value { StringValue = kvp.Value }
                        )
                    }
                };

                await _client.UpsertAsync(collectionName, [point]);
            }
            catch (Exception exc)
            {
                Console.WriteLine(exc.Message);
            }

        }
        public async Task InsertManyAsync(List<(string VectorId, float[] Vector, Dictionary<string, string> Metadata)> batch, string collectionName)
        {
            var points = batch.Select(entry =>
            {
                var metadataWithId = new Dictionary<string, string>(entry.Metadata)
                {
                    ["documentId"] = entry.VectorId,
                    ["vectorId"] = entry.VectorId
                };

                return new PointStruct
                {
                    Id = Guid.NewGuid(),
                    Vectors = entry.Vector,
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
        public async Task<List<VectorQueryResult>> QueryAsync(float[] vector, string collectionName, int topK)
        {
            var searchParams = new SearchParams { Exact = false, HnswEf = 128 };

            var response = await _client.QueryAsync(
                collectionName: collectionName,
                query: vector,
                filter: null,
                searchParams: searchParams,
                limit: (ulong)topK
            );

            // Transform the Qdrant response to match the VectorQueryResult interface.
            var results = response.Select(point => new VectorQueryResult
            {
                VectorId = point.Payload.TryGetValue("vectorId", out Value? value) ? value.StringValue : string.Empty,
                Score = point.Score,
            }).ToList();

            return results;
        }

        public async Task<List<VectorQueryResult>> QueryMultipleAsync(List<float[]> vectors, string collectionName, int topK)
        {
            var tasks = vectors.Select(vector => QueryAsync(vector, collectionName, topK));

            var results = await Task.WhenAll(tasks);

            return [.. results.SelectMany(r => r)];
        }

        public async Task<List<VectorQueryResult>> QueryHybridMedianAsync(List<float[]> queryVectors, string collectionName, int topK)
        {
            if (queryVectors == null || queryVectors.Count == 0)
            {
                throw new ArgumentException("At least one query vector is required.");
            }

            int vectorSize = queryVectors.First().Length;
            float[] combinedVector = new float[vectorSize];

            // Combine the vectors by summing them
            foreach (var vector in queryVectors)
            {
                for (int i = 0; i < vectorSize; i++)
                {
                    combinedVector[i] += vector[i];
                }
            }

            // Compute the average vector
            for (int i = 0; i < vectorSize; i++)
            {
                combinedVector[i] /= queryVectors.Count;
            }

            // Use the combined vector to perform the search.
            return await QueryAsync(combinedVector, collectionName, topK);
        }
    }
}