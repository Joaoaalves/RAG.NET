using RAGNET.Domain.Chunkers;

using RAGNET.Application.Providers;

namespace RAGNET.Application.Chunkers
{
    public interface ITextChunkerFactory
    {
        ITextChunkerService CreateChunker(Chunker chunkerConfig, IChatCompletionService completionService);
    }
}