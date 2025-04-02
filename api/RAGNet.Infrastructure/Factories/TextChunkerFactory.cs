using RAGNET.Domain.Entities;
using RAGNET.Domain.Enums;
using RAGNET.Domain.Factories;
using RAGNET.Domain.Services;
using RAGNET.Infrastructure.Adapters.Chunking;

namespace RAGNET.Infrastructure.Factories
{
    public class TextChunkerFactory : ITextChunkerFactory
    {
        private readonly IPromptService _promptService;
        public TextChunkerFactory(IPromptService promptService)
        {
            _promptService = promptService;
        }

        public ITextChunkerService CreateChunker(Chunker chunkerConfig, IChatCompletionService completionService)
        {
            int chunkSize = 600;
            int overlap = 100;
            float threshold = 0.8f;

            if (chunkerConfig.Metas != null && chunkerConfig.Metas.Count != 0)
            {
                var metaDict = chunkerConfig.Metas.ToDictionary(m => m.Key, m => m.Value);
                if (metaDict.TryGetValue("chunkSize", out string? size) && int.TryParse(size, out int parsedChunkSize))
                    chunkSize = parsedChunkSize;
                if (metaDict.TryGetValue("overlap", out string? ovrlp) && int.TryParse(ovrlp, out int parsedOverlap))
                    overlap = parsedOverlap;
                if (metaDict.TryGetValue("threshold", out string? thrshld) && float.TryParse(thrshld, out float parsedThreshold))
                    threshold = parsedThreshold;
            }

            return chunkerConfig.StrategyType switch
            {
                ChunkerStrategy.PROPOSITION => new PropositionChunkerAdapter(
                    threshold,
                    _promptService.GetPrompt("PropositionChunker", "chunker"),
                    _promptService.GetPrompt("PropositionChunker", "evaluation"),
                    completionService
                ),

                ChunkerStrategy.SEMANTIC => new SemanticChunkerAdapter(threshold, completionService),
                ChunkerStrategy.PARAGRAPH => new ParagraphChunkerAdapter(chunkSize),
                _ => throw new NotSupportedException("Estratégia de chunking não suportada."),
            };
        }
    }
}