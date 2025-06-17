namespace RAGNET.Application.Providers
{
    public interface IEmbeddingService
    {
        Task<float[]> GetEmbeddingAsync(string text);
        Task<List<float[]>> GetMultipleEmbeddingAsync(List<string> texts);
    }
}