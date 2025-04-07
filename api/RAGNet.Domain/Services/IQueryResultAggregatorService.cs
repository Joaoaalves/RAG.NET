using RAGNET.Domain.Entities;

namespace RAGNET.Domain.Services
{
    public interface IQueryResultAggregatorService
    {
        VectorQueryResult?[] AggregateResults(List<VectorQueryResult> results, int topK);
    }
}