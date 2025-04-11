using RAGNET.Domain.Entities;
using RAGNET.Domain.Services;
using RAGNET.Domain.Services.Query;

namespace RAGNET.Domain.Factories
{
    public interface IQueryEnhancerFactory
    {
        IQueryEnhancerService CreateQueryEnhancer(QueryEnhancer enhancerConfig, IChatCompletionService completionService);
    }
}