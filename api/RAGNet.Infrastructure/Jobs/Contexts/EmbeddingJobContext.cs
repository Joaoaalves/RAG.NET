using Microsoft.Extensions.DependencyInjection;

using RAGNET.Domain.Documents;
using RAGNET.Domain.Documents.Pages.Chunks;
using RAGNET.Domain.Workflows;

using RAGNET.Infrastructure.DocumentProcessors;

namespace RAGNET.Infrastructure.Jobs.Contexts
{
    public class EmbeddingJobContext(IServiceScope scope) : JobProcessingContext(scope)
    {
        public Workflow Workflow { get; set; } = null!;
        public DocumentExtractResult? ExtractResult { get; set; }
        public Document? Document { get; set; }
        public IEnumerable<Chunk> Chunks { get; set; } = [];
        public int TotalProcessed { get; set; }
    }
}