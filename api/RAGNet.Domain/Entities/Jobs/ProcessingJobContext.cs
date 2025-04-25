using Microsoft.Extensions.DependencyInjection;

namespace RAGNET.Domain.Entities.Jobs
{
    public abstract class JobProcessingContext(IServiceScope scope)
    {
        public IServiceScope Scope { get; } = scope;
    }
}