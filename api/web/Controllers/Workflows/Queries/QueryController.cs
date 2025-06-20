using Microsoft.AspNetCore.Mvc;

using RAGNET.Domain.Workflows;

using RAGNET.Application.Filters;
using RAGNET.Infrastructure.Processing;
using RAGNET.Application.QueryEnhancers.EnhanceQuery;
using RAGNET.Application.Queries.QueryChunks;
using RAGNET.Application.Queries.FilterQueryResult;
using RAGNET.Application.Queries;

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
                var workflow = HttpContext.Items["Workflow"] as Workflow
                    ?? throw new Exception("Workflow not found in context.");

                var enhanceQueryCommand = new EnhanceQueryCommand(
                    workflow,
                    queryDTO
                );

                var queries = await _commandsExecutor.Execute(enhanceQueryCommand);

                var queryChunksCommand = new QueryChunksCommand(
                    workflow,
                    queries,
                    queryDTO
                );

                var chunks = await _commandsExecutor.Execute(queryChunksCommand);
                Console.WriteLine($"Chunks length: {chunks.Count}");
                var filterQueryResult = new FilterQueryResultCommand(
                    chunks,
                    workflow,
                    queryDTO.Query
                );

                var filteredContent = await _commandsExecutor.Execute(filterQueryResult);

                return Ok(new { Chunks = chunks, FilteredContent = filteredContent });
            }
            catch (Exception exc)
            {
                return Problem(exc.Message);
            }
        }
    }
}