namespace RAGNET.Domain.Services
{
    public class VectorQueryResult
    {
        public string DocumentId { get; set; } = string.Empty;
        public double Score { get; set; }
        public Dictionary<string, string> Metadata { get; set; } = [];
    }

    public interface IVectorDatabaseService
    {
        Task CreateCollectionAsync(Guid collectionName, int vectorSize);
        Task InsertAsync(string documentId, float[] vector, string collectionName, Dictionary<string, string> metadata);
        Task<List<VectorQueryResult>> QueryAsync(float[] vector, string collectionId, int topK);
        Task<List<VectorQueryResult>> QueryMultipleAsync(List<float[]> vectors, string collectionName, int topK);
        Task<List<VectorQueryResult>> QueryHybridMedianAsync(List<float[]> queryVectors, string collectionName, int topK);
    }
}