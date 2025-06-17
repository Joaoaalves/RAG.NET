using RAGNET.Application.DTOs.QueryEnhancer;
using RAGNET.Domain.Enums;
using RAGNET.Domain.QueryEnhancers;
using RAGNET.Domain.Workflows;

namespace RAGNET.Application.Mappers
{
    public static class QueryEnhancerMapper
    {
        // QueryEnhancerDTO -> QueryEnhancer via Builder
        public static QueryEnhancer ToQueryEnhancer(this QueryEnhancerDTO dto, Guid workflowId, string userId)
        {
            return new QueryEnhancerBuilder()
                .WithId(dto.Id)
                .WithType(dto.Type)
                .WithWorkflowId(workflowId)
                .WithMaxQueries(dto.MaxQueries)
                .WithMetas(
                [
                    new("Guidance", dto.Guidance ?? "")
                ])
                .Enabled(dto.IsEnabled)
                .ForUser(userId)
                .Build();
        }

        // AutoQueryCreationDTO -> QueryEnhancer via Builder style (adapted)
        public static QueryEnhancer ToQueryEnhancer(this AutoQueryCreationDTO dto, Guid workflowId, string userId)
        {
            return new QueryEnhancerBuilder()
                .WithType(QueryEnhancerStrategy.AUTO_QUERY)
                .WithMaxQueries(dto.MaxQueries)

                .WithWorkflowId(workflowId)
                .Enabled(dto.IsEnabled ?? true)
                .ForUser(userId)
                .WithMetas(
                [
                    new("Guidance", dto.Guidance ?? "")
                ])
                .Build();
        }

        // HyDECreationDTO -> QueryEnhancer via Builder style (adapted)
        public static QueryEnhancer ToQueryEnhancer(this HyDECreationDTO dto, Guid workflowId, string userId)
        {
            return new QueryEnhancerBuilder()
                .WithType(QueryEnhancerStrategy.HYPOTHETICAL_DOCUMENT_EMBEDDING)
                .WithWorkflowId(workflowId)
                .Enabled(dto.IsEnabled ?? true)
                .ForUser(userId)
                .WithMaxQueries(dto.MaxQueries)
                .Build();
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

        public static List<QueryEnhancerDTO> ToDTOList(this IReadOnlyCollection<QueryEnhancer> queryEnhancers)
        {
            var dtoList = new List<QueryEnhancerDTO>();

            foreach (var qe in queryEnhancers)
            {
                dtoList.Add(qe.ToQueryEnhancerDTO());
            }
            return dtoList;
        }
    }
}
