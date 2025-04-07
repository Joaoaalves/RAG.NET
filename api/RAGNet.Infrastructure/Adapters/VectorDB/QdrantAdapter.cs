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
            // The DocumentId is assumed to be stored in the payload under "documentId".
            var results = response.Select(point => new VectorQueryResult
            {
                DocumentId = point.Payload.TryGetValue("documentId", out Value? value) ? value.StringValue : string.Empty,
                Score = point.Score,
            }).ToList(); // Convert to List<VectorQueryResult>

            return results;
        }

    }
}