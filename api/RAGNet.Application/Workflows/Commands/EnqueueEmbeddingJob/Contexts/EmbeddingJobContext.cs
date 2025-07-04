using Microsoft.Extensions.DependencyInjection;
using RAGNET.Application.Chunkers.Services;
using RAGNET.Application.Infrastructure.Providers.Conversation;
using RAGNET.Application.Infrastructure.Providers.Embedding;
using RAGNET.Application.Workflows.Commands.EnqueueEmbeddingJob.DocumentProcessors;
using RAGNET.Domain.Documents;
using RAGNET.Domain.Documents.Pages.Chunks;
using RAGNET.Domain.Users;
using RAGNET.Domain.Workflows;

namespace RAGNET.Application.Workflows.Commands.EnqueueEmbeddingJob.Contexts
{
    public class EmbeddingJobContext(IServiceScope scope) : JobProcessingContext(scope)
    {
        public User User { get; set; } = null!;
        public Workflow Workflow { get; set; } = null!;
        public IEmbeddingService EmbeddingProviderService { get; set; } = null!;
        public IConversationProviderService ConversationProviderService { get; set; } = null!;
        public ITextChunkerService TextChunkerService { get; set; } = null!;
        public Document Document { get; set; } = null!;
        public IEnumerable<Chunk> Chunks { get; set; } = [];
        public int TotalProcessed { get; set; }
    }
}