using RAGNET.Application.Providers.Conversation;
using RAGNET.Domain.Chunkers;


namespace RAGNET.Application.Chunkers
{
    public interface ITextChunkerFactory
    {
        ITextChunkerService CreateChunker(Chunker chunkerConfig, IChatCompletionService completionService);
    }
}