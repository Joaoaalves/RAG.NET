using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

using RAGNET.Domain.Workflows;

using RAGNET.Infrastructure.Jobs.Queue;
using RAGNET.Infrastructure.Jobs;
using RAGNET.Infrastructure.Exceptions;
using RAGNET.Infrastructure.Processing;

using web.Filters;
using RAGNET.Application.Workflows.Commands.CreateWorkflow;
using RAGNET.Application.Workflows.Commands.DeleteWorkflow;
using RAGNET.Application.Workflows.Queries.GetWorkflowDetails;
using RAGNET.Application.Workflows.Commands.UpdateWorkflow;
using RAGNET.Application.Workflows.Queries.GetUserWorkflows;
using RAGNET.Application.Workflows.CallbackUrls.Mappers;

namespace web.Controllers.Workflows
{
    [Route("api/workflows")]
    [ApiController]
    public class WorkflowController(
        CommandsExecutor commandsExecutor,
        QueriesExecutor queriesExecutor) : ControllerBase
    {
        private readonly CommandsExecutor _commandExecutor = commandsExecutor;
        private readonly QueriesExecutor _queriesExecutor = queriesExecutor;

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateWorkflow([FromBody] WorkflowCreationDTO dto)
        {
            try
            {
                Console.WriteLine(
                    dto.EmbeddingProvider.VectorSize
                );
                var command = new CreateWorkflowCommand(dto);
                var workflowId = await _commandExecutor.Execute(command);

                return Ok(new { Message = "Workflow created!", WorkflowId = workflowId });
            }
            catch (InvalidEmbeddingModelException exc)
            {
                return BadRequest(new { exc.Message });
            }
            catch (Exception e)
            {
                return Problem(e.Message);
            }
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetWorkflows()
        {
            var query = new GetUserWorkflowsQuery();
            var workflows = await _queriesExecutor.Execute(query);

            return Ok(new { Workflows = workflows });
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetWorkflow([FromRoute] Guid id)
        {
            try
            {
                var query = new GetWorkflowDetailsQuery(
                    new WorkflowId(id)
                );

                var workflowDetails = await _queriesExecutor.Execute(query);

                return Ok(workflowDetails);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> UpdateWorkflow([FromBody] WorkflowDetailsUpdateDTO dto, [FromRoute] Guid id)
        {
            try
            {
                var command = new UpdateWorkflowCommand(
                    new WorkflowId(id),
                    dto.Name,
                    dto.Description,
                    dto.IsActive,
                    dto.EmbeddingProvider,
                    dto.ConversationProvider
                );

                var workflow = await _commandExecutor.Execute(command);

                return Ok(new { workflow });
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteWorkflow([FromRoute] Guid id)
        {
            try
            {
                var command = new DeleteWorkflowCommand(
                    new WorkflowId(id)
                );

                var deleted = await _commandExecutor.Execute(command);

                return Ok(new { Message = "Workflow Deleted!", WorkflowId = id });
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPost("embedding")]
        [Consumes("multipart/form-data")]
        [ServiceFilter(typeof(ApiWorkflowFilter))]
        public async Task ProcessEmbedding(IFormFile file, [FromServices] IEmbeddingJobQueue enqueuer, CancellationToken cancellationToken, [FromQuery] bool stream = false)
        {
            try
            {
                var workflow = HttpContext.Items["Workflow"] as Workflow
                    ?? throw new InvalidOperationException("Workflow not found on context.");

                if (!workflow.IsActive)
                {
                    throw new Exception("Workflow is not active!");
                }

                var ms = new MemoryStream();
                file.CopyTo(ms);

                var urls = workflow.CallbackUrls.Select(curl => curl.Url).ToList();
                var job = new EmbeddingJob
                {
                    ApiKey = workflow.ApiKey,
                    UserId = workflow.UserId,
                    FileName = file.FileName,
                    FileContent = ms.ToArray(),
                    CallbackUrls = urls.ToUrlList()
                };

                await enqueuer.EnqueueAsync(job, cancellationToken);

                Response.StatusCode = 202; // Accepted
                await Response.WriteAsJsonAsync(new
                {
                    Message = "Job queued.",
                    job.JobId
                });
            }
            catch (Exception ex)
            {
                Response.StatusCode = 400;
                await Response.WriteAsync(ex.Message);
            }
        }
    }
}
