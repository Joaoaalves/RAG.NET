using RAGNET.Domain.Entities;

namespace RAGNET.Domain.Services
{
    public interface IQueryResultAggregatorService
    {
        List<VectorQueryResult> AggregateResults(List<VectorQueryResult> results, int topK = 5);
    }
}