using RAGNET.Domain.Entities;
using RAGNET.Domain.Enums;
using RAGNET.Domain.Factories;
using RAGNET.Domain.Services;
using RAGNET.Infrastructure.Adapters.Chunking;

namespace RAGNET.Infrastructure.Factories
{
    public class TextChunkerFactory : ITextChunkerFactory
    {
        public ITextChunkerService CreateChunker(Chunker chunkerConfig)
        {
            int chunkSize = 600;
            int overlap = 100;

            if (chunkerConfig.Metas != null && chunkerConfig.Metas.Any())
            {
                var metaDict = chunkerConfig.Metas.ToDictionary(m => m.Key, m => m.Value);
                if (metaDict.TryGetValue("chunkSize", out string? value) && int.TryParse(value, out int parsedChunkSize))
                    chunkSize = parsedChunkSize;
                if (metaDict.ContainsKey("overlap") && int.TryParse(metaDict["overlap"], out int parsedOverlap))
                    overlap = parsedOverlap;
            }

            return chunkerConfig.StrategyType switch
            {
                ChunkerStrategy.PROPOSITION => new PropositionChunkerAdapter(chunkSize, overlap),
                ChunkerStrategy.SEMANTIC => new SemanticChunkerAdapter(chunkSize, overlap),
                _ => throw new NotSupportedException("Estratégia de chunking não suportada."),
            };
        }
    }
}