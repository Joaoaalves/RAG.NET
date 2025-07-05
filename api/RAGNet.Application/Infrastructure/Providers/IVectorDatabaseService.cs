using RAGNET.Application.Infrastructure.Providers.Embedding.DTOs;
using RAGNET.Domain.Documents.Pages.Chunks;

namespace RAGNET.Application.Infrastructure.Providers
{
    public class VectorQueryResult
    {
        public string VectorId { get; set; } = string.Empty;
        public double Score { get; set; }
        public Dictionary<string, string> Metadata { get; set; } = [];
    }

    public interface IVectorDatabaseService
    {
        Task CreateCollectionAsync(Guid collectionName, int vectorSize);
        Task InsertAsync(EmbeddingDTO embedding, string collectionName);
        Task InsertManyAsync(IEnumerable<EmbeddingDTO> batch, string collectionName);
        Task<List<VectorQueryResult>> QueryAsync(SemanticVector vector, string collectionId, int topK);
        Task<List<VectorQueryResult>> QueryMultipleAsync(IEnumerable<SemanticVector> vectors, string collectionName, int topK);
        Task<List<VectorQueryResult>> QueryByAverageVectorAsync(IEnumerable<SemanticVector> queryVectors, string collectionName, int topK);
    }
}