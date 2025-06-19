using RAGNET.Application.DTOs.QueryResultFilter;
using RAGNET.Domain.QueryResultFilters;
using RAGNET.Domain.Workflows;

namespace RAGNET.Application.QueryResultFilters
{
    public static class QueryResultFilterMapper
    {
        public static QueryResultFilter ToFilter(this RSECreationDTO dto, WorkflowId workflowId, string userId)
        {
            return QueryResultFilter.Create(
                strategy: QueryResultFilterStrategyEnum.RELEVANT_SEGMENT_EXTRACTION,
                workflowId: workflowId,
                isEnabled: dto.IsEnabled ?? false,
                maxItems: dto.MaxItems,
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