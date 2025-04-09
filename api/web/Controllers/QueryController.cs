using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RAGNET.Application.Attributes;
using RAGNET.Application.DTOs.Chunk;
using RAGNET.Application.DTOs.Query;
using RAGNET.Application.Mappers;
using RAGNET.Application.UseCases.Query;
using RAGNET.Application.UseCases.QueryEnhancerUseCases;
using RAGNET.Domain.Entities;

namespace web.Controllers
{
    [Route("/api/")]
    [ApiController]
    public class QueryController(
        IEnhanceQueryUseCase enhanceQueryUseCase,
        IQueryChunksUseCase queryChunksUseCase) : ControllerBase
    {
        private readonly IEnhanceQueryUseCase _enhanceQueryUseCase = enhanceQueryUseCase;
        private readonly IQueryChunksUseCase _queryChunksUseCase = queryChunksUseCase;

        [HttpPost("query")]
        [ApiKeyCheck]
        public async Task<IActionResult> Query([FromBody] QueryDTO queryDTO)
        {
            try
            {
                var apiKey = Request.Headers["x-api-key"].ToString();
                var queries = await _enhanceQueryUseCase.Execute(apiKey, queryDTO);

                // Now the user query is always sent to retrieval
                // TODO: This must be set by the user
                queries.Add(queryDTO.Query);

                var chunksResult = await _queryChunksUseCase.Execute(apiKey, queries, queryDTO.TopK);

                // TODO:
                // Filter results before ranking.

                List<ChunkDTO> chunks = [];

                foreach (var c in chunksResult)
                {
                    chunks.Add(c.ToDTO());
                }
                return Ok(new { chunks });
            }
            catch (Exception exc)
            {
                return Problem(exc.Message);
            }
        }
    }
}