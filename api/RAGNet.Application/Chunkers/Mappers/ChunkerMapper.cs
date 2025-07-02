using RAGNET.Application.Chunkers.DTOs;

namespace RAGNET.Application.Chunkers.Mappers
{
    public static class ChunkerMapper
    {
        public static ChunkerSettingsDTO ToChunkerSettingsDTO(this Dictionary<string, string> meta)
        {
            return new ChunkerSettingsDTO
            {
                Threshold = double.Parse(meta["threshold"]),
                EvaluationPrompt = meta["evaluationPrompt"],
                MaxChunkSize = int.Parse(meta["maxChunkSize"])
            };
        }
    }
}