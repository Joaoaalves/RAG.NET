using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// Domain
using RAGNET.Domain.QueryEnhancers;

using web.Filters;
using RAGNET.Application.QueryEnhancers.Commands.CreateQueryEnhancer;
using RAGNET.Application.QueryEnhancers.Commands.UpdateQueryEnhancer;
using RAGNET.Application.QueryEnhancers.Commands.DeleteQueryEnhancer;

using RAGNET.Infrastructure.Processing;

namespace web.Controllers.Workflows.QueryEnhancers
{
    [Route("api/workflows")]
    [ApiController]
    public class QueryEnhancerController(
        CommandsExecutor commandsExecutor) : ControllerBase
    {
        private readonly CommandsExecutor _commandsExecutor = commandsExecutor;

        [HttpPost("{workflowId}/query-enhancer/auto-query")]
        [Authorize]
        [ServiceFilter(typeof(WebWorkflowFilter))]
        public async Task<IActionResult> EnableAutoQuery([FromBody] CreateAutoQueryRequest request, [FromRoute] Guid workflowId)
        {
            try
            {
                var command = new CreateQueryEnhancerCommand(
                    QueryEnhancerStrategy.AUTO_QUERY,
                    request.MaxQueries,
                    request.Guidance
                );

                var queryEnhancer = await _commandsExecutor.Execute(command);

                return Ok(new { Message = "Auto Query enabled!", queryEnhancer });
            }
            catch (Exception exc)
            {
                return Problem(exc.Message);
            }
        }

        [HttpPut("{workflowId}/query-enhancer/auto-query")]
        [Authorize]
        [ServiceFilter(typeof(WebWorkflowFilter))]
        public async Task<IActionResult> UpdateAutoQuery([FromBody] UpdateAutoQueryRequest request, [FromRoute] Guid workflowId)
        {
            try
            {
                var command = new UpdateQueryEnhancerCommand(
                    QueryEnhancerStrategy.AUTO_QUERY,
                    request.MaxQueries,
                    request.IsEnabled,
                    request.Guidance
                );

                var queryEnhancer = await _commandsExecutor.Execute(command);

                return Ok(new { Message = "Auto Query updated!", queryEnhancer });
            }
            catch (Exception exc)
            {
                return Problem(exc.Message);
            }
        }

        [HttpDelete("{workflowId}/query-enhancer/auto-query")]
        [Authorize]
        [ServiceFilter(typeof(WebWorkflowFilter))]
        public async Task<IActionResult> DisableAutoQuery([FromRoute] Guid workflowId)
        {
            try
            {
                var command = new DeleteQueryEnhancerCommand(
                    QueryEnhancerStrategy.AUTO_QUERY
                );

                var success = await _commandsExecutor.Execute(command);

                if (!success)
                    return BadRequest("Something went wrong, Auto Query was not disabled!");

                return Ok(new { Message = "Auto Query disabled!" });
            }
            catch (Exception exc)
            {
                return Problem(exc.Message);
            }
        }

        [HttpPost("{workflowId}/query-enhancer/hyde")]
        [Authorize]
        [ServiceFilter(typeof(WebWorkflowFilter))]
        public async Task<IActionResult> EnableHyde([FromBody] CreateHyDERequest request, [FromRoute] Guid workflowId)
        {
            try
            {
                var command = new CreateQueryEnhancerCommand(
                    QueryEnhancerStrategy.HYPOTHETICAL_DOCUMENT_EMBEDDING,
                    request.MaxQueries
                );

                var queryEnhancer = await _commandsExecutor.Execute(command);

                return Ok(new { Message = "Hyde enabled!", queryEnhancer });
            }
            catch (Exception exc)
            {
                return Problem(exc.Message);
            }
        }

        [HttpPut("{workflowId}/query-enhancer/hyde")]
        [Authorize]
        [ServiceFilter(typeof(WebWorkflowFilter))]
        public async Task<IActionResult> UpdateHyde([FromBody] UpdateHyDERequest request, [FromRoute] Guid workflowId)
        {
            try
            {
                var command = new UpdateQueryEnhancerCommand(
                    QueryEnhancerStrategy.HYPOTHETICAL_DOCUMENT_EMBEDDING,
                    request.MaxQueries,
                    request.IsEnabled
                );

                var queryEnhancer = await _commandsExecutor.Execute(command);

                return Ok(new { Message = "Hyde updated!", queryEnhancer });
            }
            catch (Exception exc)
            {
                return Problem(exc.Message);
            }
        }

        [HttpDelete("{workflowId}/query-enhancer/hyde")]
        [Authorize]
        [ServiceFilter(typeof(WebWorkflowFilter))]
        public async Task<IActionResult> DisableHyde([FromRoute] Guid workflowId)
        {
            try
            {
                var command = new DeleteQueryEnhancerCommand(
                    QueryEnhancerStrategy.HYPOTHETICAL_DOCUMENT_EMBEDDING
                );

                var sucess = await _commandsExecutor.Execute(command);

                if (!sucess)
                    return BadRequest("Something went wrong, HyDE was not disabled!");

                return Ok(new { Message = "Hyde disabled!" });
            }
            catch (Exception exc)
            {
                return Problem(exc.Message);
            }
        }
    }
}