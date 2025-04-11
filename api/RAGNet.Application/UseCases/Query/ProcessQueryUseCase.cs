using RAGNET.Application.DTOs.Chunk;
using RAGNET.Application.DTOs.Query;
using RAGNET.Application.Mappers;
using RAGNET.Application.UseCases.Query;
using RAGNET.Application.UseCases.QueryEnhancerUseCases;
using RAGNET.Domain.Entities;

namespace RAGNET.Application.UseCases
{
    public interface IProcessQueryUseCase
    {
        Task<List<ChunkDTO>> Execute(Workflow workflow, QueryDTO queryDTO);
    }

    public class ProcessQueryUseCase(
        IEnhanceQueryUseCase enhanceQueryUseCase,
        IQueryChunksUseCase queryChunksUseCase
    ) : IProcessQueryUseCase
    {
        public async Task<List<ChunkDTO>> Execute(Workflow workflow, QueryDTO queryDTO)
        {
            var queries = await enhanceQueryUseCase.Execute(workflow, queryDTO);

            // Fallback if no queries was generated
            if (queries.Count == 0)
            {
                queries.Add(queryDTO.Query);
            }


            // TODO:
            // Filter results before ranking.

            var chunks = await queryChunksUseCase.Execute(workflow, queries, queryDTO);

            return chunks.Select(c => c.ToDTO()).ToList();

        }
    }
}