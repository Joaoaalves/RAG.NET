using RAGNET.Application.DTOs.ContentFilter;
using RAGNET.Domain.Entities;
using RAGNET.Domain.Enums;

namespace RAGNET.Application.Mappers
{
    public static class ContentFilterMapper
    {
        public static Filter ToFilter(this RSECreationDTO dto, Guid workflowId, string userId)
        {
            return new Filter
            {
                Strategy = FilterStrategy.RELEVANT_SEGMENT_EXTRACTION,
                WorkflowId = workflowId,
                UserId = userId,
                Metas =
                [
                    new() {Key = "MaximumItems", Value = dto.MaxItems.ToString()}
                ]
            };
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