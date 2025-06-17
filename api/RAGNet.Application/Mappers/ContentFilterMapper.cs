using RAGNET.Application.DTOs.ContentFilter;
using RAGNET.Domain.Filters;

namespace RAGNET.Application.Mappers
{
    public static class ContentFilterMapper
    {
        public static Filter ToFilter(this RSECreationDTO dto, Guid workflowId, string userId)
        {
            return Filter.Create(
                id: Guid.NewGuid(),
                strategy: FilterStrategyEnum.RELEVANT_SEGMENT_EXTRACTION,
                workflowId: workflowId,
                isEnabled: dto.IsEnabled ?? false,
                maxItems: dto.MaxItems,
                userId: userId
            );
        }

        public static FilterDTO ToDTO(this Filter filter)
        {
            return new FilterDTO
            {
                Id = filter.Id,
                Strategy = filter.Strategy,
                MaxItems = filter.MaxItems,
                IsEnabled = filter.IsEnabled
            };
        }
    }
}