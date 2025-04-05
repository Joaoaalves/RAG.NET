using RAGNET.Application.DTOs.QueryEnhancer;
using RAGNET.Domain.Entities;
using RAGNET.Domain.Enums;

namespace RAGNET.Application.Mappers
{
    public static class QueryEnhancerMapper
    {
        // QueryEnhancerDTO -> QueryEnhancer
        public static QueryEnhancer ToQueryEnhancer(this QueryEnhancerDTO dto, Guid workflowId, string userId)
        {
            return new QueryEnhancer
            {
                Id = dto.Id,
                Type = dto.Type,
                WorkflowId = workflowId,
                MaxQueries = dto.MaxQueries,
                IsEnabled = dto.IsEnabled,
                UserId = userId,
                Metas =
                [
                    new() {Key = "Guidance", Value = dto.Guidance ?? ""}
                ]
            };
        }


        // QueryEnhancerDTO -> QueryEnhancer
        public static QueryEnhancer ToQueryEnhancer(this QueryEnhancerDTO dto, Guid workflowId)
        {
            return new QueryEnhancer
            {
                Id = dto.Id,
                Type = dto.Type,
                WorkflowId = workflowId,
                MaxQueries = dto.MaxQueries,
                IsEnabled = dto.IsEnabled,
                Metas =
                [
                    new() {Key = "Guidance", Value = dto.Guidance ?? ""}
                ]
            };
        }
        // AutoQueryCreationDTO -> QueryEnhancerDTO
        public static QueryEnhancer ToQueryEnhancer(this AutoQueryCreationDTO dto, Guid workflowId, string userId)
        {
            return new QueryEnhancer
            {
                Type = QueryEnhancerStrategy.AUTO_QUERY,
                MaxQueries = dto.MaxQueries,
                WorkflowId = workflowId,
                IsEnabled = dto.IsEnabled ?? true,
                UserId = userId,
                Metas =
                [
                    new() {Key = "Guidance", Value = dto.Guidance ?? ""}
                ]
            };
        }

        // HyDECreationDTO -> QueryEnhancerDTO
        public static QueryEnhancer ToQueryEnhancer(this HyDECreationDTO dto, Guid workflowId, string userId)
        {
            return new QueryEnhancer
            {
                Type = QueryEnhancerStrategy.HYPOTHETICAL_DOCUMENT_EMBEDDING,
                WorkflowId = workflowId,
                IsEnabled = dto.IsEnabled ?? true,
                UserId = userId,
                MaxQueries = dto.MaxQueries,
            };
        }

        // QueryEnhancer -> QueryEnhancerDTO
        public static QueryEnhancerDTO ToQueryEnhancerDTO(this QueryEnhancer qe)
        {
            return new QueryEnhancerDTO
            {
                Id = qe.Id,
                Type = qe.Type,
                IsEnabled = qe.IsEnabled,
                MaxQueries = qe.MaxQueries,
                Guidance = qe.Metas.FirstOrDefault(m => m.Key == "Guidance")?.Value
            };
        }

    }
}