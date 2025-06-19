using RAGNET.Application.QueryEnhancers.CreateQueryEnhancer;
using RAGNET.Application.QueryEnhancers.UpdateQueryEnhancer;
using RAGNET.Domain.QueryEnhancers;
using RAGNET.Domain.Workflows;

namespace RAGNET.Application.QueryEnhancers
{
    public static class QueryEnhancerMapper
    {
        // QueryEnhancerDTO -> QueryEnhancer via Builder
        public static QueryEnhancer ToQueryEnhancer(this QueryEnhancerDTO dto, WorkflowId workflowId, string userId)
        {
            return new QueryEnhancerBuilder()
                .WithType(dto.Type)
                .WithWorkflowId(workflowId)
                .WithMaxQueries(dto.MaxQueries)
                .WithMetas(
                [
                    new("Guidance", dto.Guidance ?? "")
                ])
                .Enabled(dto.IsEnabled)
                .ForUser(userId)
                .Build(
                    new QueryEnhancerId(dto.Id)
                );
        }

        // CreateAutoQueryRequest -> QueryEnhancer via Builder style (adapted)
        public static QueryEnhancer ToQueryEnhancer(this CreateAutoQueryRequest dto, WorkflowId workflowId, string userId)
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

        // CreateHyDERequest -> QueryEnhancer via Builder style (adapted)
        public static QueryEnhancer ToQueryEnhancer(this CreateHyDERequest dto, WorkflowId workflowId, string userId)
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
        public static QueryEnhancerDTO ToDTO(this QueryEnhancer qe)
        {
            return new QueryEnhancerDTO
            {
                Id = qe.Id.Value,
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
                dtoList.Add(qe.ToDTO());
            }
            return dtoList;
        }
    }
}
