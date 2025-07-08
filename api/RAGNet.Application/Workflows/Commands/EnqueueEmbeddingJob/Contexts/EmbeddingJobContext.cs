using Microsoft.Extensions.DependencyInjection;

using RAGNET.Application.Chunkers.Services;
using RAGNET.Application.Infrastructure.Providers;
using RAGNET.Application.Infrastructure.Providers.Conversation;
using RAGNET.Application.Infrastructure.Providers.Embedding;
using RAGNET.Domain.Chunkers;
using RAGNET.Domain.Documents;
using RAGNET.Domain.Documents.Pages.Chunks;
using RAGNET.Domain.TokenWallets;
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
        public IVectorDatabaseService VectorDatabaseService { get; set; } = null!;
        public ITextChunkerService TextChunkerService { get; set; } = null!;
        public Document Document { get; set; } = null!;
        public IEnumerable<Chunk> Chunks { get; set; } = [];
        public int TotalProcessed { get; set; }

        // Consume Tokens Handler
        public void Deconstruct(out Workflow workflow, out TokenWallet tokenWallet, out ITextChunkerService chunkerService, out int totalPages)
        {
            workflow = Workflow;
            tokenWallet = User.TokenWallet;
            chunkerService = TextChunkerService;
            totalPages = Document.Pages.Count;
        }

        // Process Pages Handler
        public void Deconstruct(out Workflow workflow, out ITextChunkerService chunkerService, out Document document)
        {
            workflow = Workflow;
            chunkerService = TextChunkerService;
            document = Document;
        }

        // Mount Chunker Handler
        public void Deconstruct(out User user, out Chunker chunker)
        {
            chunker = Workflow.Chunker;
            user = User;
        }
    }
}