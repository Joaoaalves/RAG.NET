using RAGNET.Domain.Entities;
using RAGNET.Domain.Services;

namespace RAGNET.Domain.Factories
{
    public interface IQueryEnhancerFactory
    {
        IQueryEnhancerService CreateQueryEnhancer(QueryEnhancer enhancerConfig, IChatCompletionService completionService);
    }
}