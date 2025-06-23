using RAGNET.Application.QueryResultFilters.CreateQueryResultFilter;
using RAGNET.Domain.QueryResultFilters;
using RAGNET.Domain.Workflows;

namespace RAGNET.Application.QueryResultFilters
{
    public static class QueryResultFilterMapper
    {
        public static QueryResultFilter ToFilter(this QueryResultFilterCreationRequest request, WorkflowId workflowId, string userId)
        {
            return QueryResultFilter.Create(
                strategy: QueryResultFilterStrategy.RELEVANT_SEGMENT_EXTRACTION,
                workflowId: workflowId,
                maxItems: request.MaxItems,
                userId: userId
            );
        }

        public static QueryResultFilterDTO ToDTO(this QueryResultFilter filter)
        {
            return new QueryResultFilterDTO
            {
                Id = filter.Id.Value,
                Strategy = filter.Strategy,
                MaxItems = filter.MaxItems,
                IsEnabled = filter.IsEnabled
            };
        }
    }
}