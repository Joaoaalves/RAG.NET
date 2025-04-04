using RAGNET.Application.DTOs.QueryEnhancer;
using RAGNET.Domain.Entities;
using RAGNET.Domain.Enums;

namespace RAGNET.Application.Mappers
{
    public static class QueryEnhancerMapper
    {
        public static QueryEnhancer ToQueryEnhancerFromCreationDTO(this QueryEnhancerCreationDTO dto, Guid workflowId)
        {
            return new QueryEnhancer
            {
                Type = dto.Type,
                WorkflowId = workflowId,
                MaxQueries = dto.MaxQueries,
                Metas =
                [
                    new() {Key = "Guidance", Value = dto.Guidance ?? ""}
                ]
            };
        }

        public static QueryEnhancerCreationDTO ToQueryEnhancerDTOFromAutoQueryCreationDTO(this AutoQueryCreationDTO dto)
        {
            return new QueryEnhancerCreationDTO
            {
                Type = QueryEnhancerStrategy.AUTO_QUERY,
                MaxQueries = dto.MaxQueries,
                Guidance = dto.Guidance
            };
        }

        public static QueryEnhancerCreationDTO ToQueryEnhancerDTOFromHyDECreationDTO(this HyDECreationDTO dto)
        {
            return new QueryEnhancerCreationDTO
            {
                Type = QueryEnhancerStrategy.HYPOTHETICAL_DOCUMENT_EMBEDDING,
                MaxQueries = dto.MaxQueries
            };
        }

        public static QueryEnhancerDTO ToDTOFromQueryEnhancer(this QueryEnhancer qe)
        {
            return new QueryEnhancerDTO
            {
                Id = qe.Id,
                Type = qe.Type,
                MaxQueries = qe.MaxQueries,
                Guidance = qe.Metas.FirstOrDefault(m => m.Key == "Guidance")?.Value
            };
        }
    }
}