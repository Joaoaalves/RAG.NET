using Microsoft.AspNetCore.Mvc;

using RAGNET.Domain.Workflows;

using RAGNET.Application.DTOs.Query;
using RAGNET.Application.Filters;
using RAGNET.Application.UseCases.Query;
using RAGNET.Infrastructure.Processing;
using RAGNET.Application.QueryEnhancers.EnhanceQuery;

namespace web.Controllers.Workflows.Queries
{
    [Route("/api/")]
    [ApiController]
    public class QueryController(
        CommandsExecutor commandsExecutor,
        IProcessQueryUseCase processQueryUseCase) : ControllerBase
    {
        private readonly IProcessQueryUseCase _processsQueryUseCase = processQueryUseCase;
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

                var (chunks, filteredContent) = await _processsQueryUseCase.Execute(workflow, queryDTO, queries);

                return Ok(new { Chunks = chunks, FilteredContent = filteredContent });
            }
            catch (Exception exc)
            {
                return Problem(exc.Message);
            }
        }
    }
}