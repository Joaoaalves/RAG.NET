using Microsoft.AspNetCore.Http;
using RAGNET.Application.Configuration.Commands;

namespace RAGNET.Application.Workflows.Commands.EnqueueEmbeddingJob
{
    public class EnqueueEmbeddingJobCommand(IFormFile File) : WorkflowAndUserAwareCommand<Guid>
    {
        public IFormFile File { get; } = File;
    }
}