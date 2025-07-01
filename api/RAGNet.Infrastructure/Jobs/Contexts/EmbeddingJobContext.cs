using Microsoft.Extensions.DependencyInjection;
using RAGNET.Application.Chunkers.Services;
using RAGNET.Application.Infrastructure.Providers.Conversation;
using RAGNET.Application.Infrastructure.Providers.Embedding;
using RAGNET.Domain.Documents;
using RAGNET.Domain.Documents.Pages.Chunks;
using RAGNET.Domain.TokenWallets;
using RAGNET.Domain.Users;
using RAGNET.Domain.Workflows;

using RAGNET.Infrastructure.DocumentProcessors;

namespace RAGNET.Infrastructure.Jobs.Contexts
{
    public class EmbeddingJobContext(IServiceScope scope) : JobProcessingContext(scope)
    {
        public Workflow Workflow { get; set; } = null!;
        public TokenWallet Wallet { get; set; } = null!;
        public IConversationProviderService ConversationProviderService { get; set; } = null!;
        public IEmbeddingService EmbeddingProviderService { get; set; } = null!;
        public ITextChunkerService TextChunkerService { get; set; } = null!;
        public User User { get; set; } = null!;
        public DocumentExtractResult? ExtractResult { get; set; }
        public Document? Document { get; set; }
        public IEnumerable<Chunk> Chunks { get; set; } = [];
        public int TotalProcessed { get; set; }
    }
}