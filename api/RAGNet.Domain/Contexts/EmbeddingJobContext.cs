using Microsoft.Extensions.DependencyInjection;
using RAGNET.Domain.Entities;
using RAGNET.Domain.Entities.Jobs;
using RAGNET.Domain.Services.Queue;

namespace RAGNET.Domain.Contexts
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