using RAGNET.Application.Infrastructure.Providers;

namespace RAGNET.Application.Queries.Services
{
    public interface IQueryResultAggregatorService
    {
        List<VectorQueryResult> AggregateResults(List<VectorQueryResult> results, double? minScore, int topK = 5);
    }
}