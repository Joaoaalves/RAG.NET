using Microsoft.Extensions.DependencyInjection;

namespace RAGNET.Infrastructure.Jobs
{
    public abstract class JobProcessingContext(IServiceScope scope)
    {
        public IServiceScope Scope { get; } = scope;
    }
}