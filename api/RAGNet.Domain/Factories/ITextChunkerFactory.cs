using RAGNET.Domain.Entities;
using RAGNET.Domain.Services;

namespace RAGNET.Domain.Factories
{
    public interface ITextChunkerFactory
    {
        ITextChunkerService CreateChunker(Chunker chunkerConfig, IChatCompletionService completionService);
    }
}