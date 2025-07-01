using Microsoft.Extensions.DependencyInjection;

namespace RAGNET.Application.Workflows.Commands.EnqueueEmbeddingJob.Contexts
{
    public abstract class JobProcessingContext(IServiceScope scope)
    {
        public IServiceScope Scope { get; } = scope;
    }
}