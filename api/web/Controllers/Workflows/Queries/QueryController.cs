using Microsoft.AspNetCore.Mvc;


using RAGNET.Application.Filters;
using RAGNET.Application.QueryEnhancers.EnhanceQuery;
using RAGNET.Application.Queries.QueryChunks;
using RAGNET.Application.Queries.FilterQueryResult;
using RAGNET.Application.Queries;

using RAGNET.Infrastructure.Processing;

namespace web.Controllers.Workflows.Queries
{
    [Route("/api/")]
    [ApiController]
    public class QueryController(
        CommandsExecutor commandsExecutor) : ControllerBase
    {
        private readonly CommandsExecutor _commandsExecutor = commandsExecutor;

        [HttpPost("query")]
        [ServiceFilter(typeof(ApiWorkflowFilter))]
        public async Task<IActionResult> Query([FromBody] QueryDTO queryDTO)
        {
            try
            {
                var enhanceQuery = new EnhanceQueryCommand(queryDTO);
                var queries = await _commandsExecutor.Execute(enhanceQuery);

                var chunkQuery = new QueryChunksCommand(queries, queryDTO);
                var chunks = await _commandsExecutor.Execute(chunkQuery);

                var filterQuery = new FilterQueryResultCommand(chunks, queryDTO.Query);
                var filteredContent = await _commandsExecutor.Execute(filterQuery);

                return Ok(new { Chunks = chunks, FilteredContent = filteredContent });
            }
            catch (Exception exc)
            {
                return Problem(exc.Message);
            }
        }
    }
}