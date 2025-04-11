using RAGNET.Application.DTOs.Query;
using RAGNET.Application.UseCases.QueryEnhancerUseCases;
using RAGNET.Domain.Entities;

namespace RAGNET.Application.UseCases.Query
{
    public interface IProcessQueryUseCase
    {
        Task<(List<ContentItem>, List<string>)> Execute(Workflow workflow, QueryDTO queryDTO);
    }

    public class ProcessQueryUseCase(
        IEnhanceQueryUseCase enhanceQueryUseCase,
        IQueryChunksUseCase queryChunksUseCase,
        IFilterContentUseCase filterContentUseCase
    ) : IProcessQueryUseCase
    {
        public async Task<(List<ContentItem>, List<string>)> Execute(Workflow workflow, QueryDTO queryDTO)
        {
            var queries = await enhanceQueryUseCase.Execute(workflow, queryDTO);

            // Fallback if no queries was generated
            if (queries.Count == 0)
            {
                queries.Add(queryDTO.Query);
            }

            var contentRetrieved = await queryChunksUseCase.Execute(workflow, queries, queryDTO);

            var filteredText = await filterContentUseCase.Execute(contentRetrieved, workflow, queryDTO.Query);

            return (contentRetrieved, filteredText);
        }
    }
}