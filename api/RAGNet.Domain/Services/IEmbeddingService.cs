namespace RAGNET.Domain.Services
{
    public interface IEmbeddingService
    {
        Task<float[]> GenerateEmbedding(string text);
    }
}