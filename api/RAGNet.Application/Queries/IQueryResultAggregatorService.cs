using RAGNET.Application.Providers;

namespace RAGNET.Application.Queries
{
    public interface IQueryResultAggregatorService
    {
        List<VectorQueryResult> AggregateResults(List<VectorQueryResult> results, double? minScore, int topK = 5);
    }
}