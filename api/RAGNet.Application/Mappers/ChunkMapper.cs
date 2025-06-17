using RAGNET.Application.DTOs.Chunk;
using RAGNET.Domain.Documents.Pages.Chunks;

namespace RAGNET.Application.Mappers
{
    public static class ChunkMapper
    {
        public static ChunkDTO ToDTO(this Chunk chunk)
        {
            return new ChunkDTO
            {
                VectorId = chunk.VectorId,
                Text = chunk.Text.Value,
                Score = chunk.GetScore()
            };
        }

    }
}