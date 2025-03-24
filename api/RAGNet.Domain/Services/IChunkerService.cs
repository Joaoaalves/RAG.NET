using RAGNET.Domain.Entities;

namespace RAGNET.Domain.Services
{
    public interface IChunkerService
    {
        Task<List<Chunk>> GenerateChunks(string text);
    }
}