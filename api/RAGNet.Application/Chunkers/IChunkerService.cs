using RAGNET.Domain.Documents.Pages.Chunks;

namespace RAGNET.Application.Chunkers
{
    public interface IChunkerService
    {
        Task<List<Chunk>> GenerateChunks(string text);
    }
}