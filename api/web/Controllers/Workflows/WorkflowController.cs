using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

using RAGNET.Domain.Users;
using RAGNET.Domain.Workflows;

using RAGNET.Infrastructure.Jobs.Queue;
using RAGNET.Infrastructure.Jobs;
using RAGNET.Infrastructure.Exceptions;
using RAGNET.Infrastructure.Processing;

using RAGNET.Application.Filters;
using RAGNET.Application.Workflows.CreateWorkflow;
using RAGNET.Application.Workflows.DeleteWorkflow;
using RAGNET.Application.Workflows.GetWorkflowDetails;
using RAGNET.Application.Workflows.UpdateWorkflow;
using RAGNET.Application.Workflows.GetUserWorkflows;
using RAGNET.Application.Workflows.CallbackUrls;

namespace web.Controllers.Workflows
{
    [Route("api/workflows")]
    [ApiController]
    public class WorkflowController(
        UserManager<User> userManager,
        CommandsExecutor commandsExecutor,
        QueriesExecutor queriesExecutor) : ControllerBase
    {
        private readonly UserManager<User> _userManager = userManager;
        private readonly CommandsExecutor _commandExecutor = commandsExecutor;
        private readonly QueriesExecutor _queriesExecutor = queriesExecutor;

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateWorkflow([FromBody] WorkflowCreationDTO dto)
        {
            try
            {
                var user = await _userManager.GetUserAsync(User);
                if (user == null)
                    return Unauthorized();

                var command = new CreateWorkflowCommand(dto, user);
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
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
                return Unauthorized();

            var query = new GetUserWorkflowsQuery(user.Id);
            var workflows = await _queriesExecutor.Execute(query);

            return Ok(new { Workflows = workflows });
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetWorkflow([FromRoute] Guid id)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return Unauthorized();

            try
            {
                var query = new GetWorkflowDetailsQuery(
                    new WorkflowId(id),
                    user.Id
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
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
                return Unauthorized();

            try
            {
                var command = new UpdateWorkflowCommand(
                    new WorkflowId(id),
                    user.Id,
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
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
                return Unauthorized();

            try
            {
                var command = new DeleteWorkflowCommand(
                    new WorkflowId(id),
                    user.Id
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
