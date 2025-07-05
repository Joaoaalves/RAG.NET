namespace RAGNET.Domain.Documents.Pages.Chunks
{
    public interface IChunkRepository
    {
        Task<Chunk?> GetByVectorId(string vectorId);
        Task<List<Chunk>> GetManyByVectorId(string[] vectorIds);
        Task<List<Chunk>> AddManyAsync(List<Chunk> chunks);

    }
}