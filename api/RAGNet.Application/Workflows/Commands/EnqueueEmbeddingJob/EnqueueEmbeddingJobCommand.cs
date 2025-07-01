using Microsoft.AspNetCore.Http;
using RAGNET.Application.Configuration.Commands;

namespace RAGNET.Application.Workflows.Commands.EnqueueEmbeddingJob
{
    public class EnqueueEmbeddingJobCommand(IFormFile File) : WorkflowAwareCommand<Guid>
    {
        public IFormFile File { get; } = File;
    }
}