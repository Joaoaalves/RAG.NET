using RAGNET.Application.Chunkers.Services;
using RAGNET.Application.Infrastructure.Providers.Conversation;
using RAGNET.Domain.Chunkers;


namespace RAGNET.Application.Chunkers.Factories
{
    public interface ITextChunkerFactory
    {
        ITextChunkerService CreateChunker(Chunker chunkerConfig, IConversationProviderService completionService);
    }
}