using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

using RAGNET.Domain.Users;
using RAGNET.Domain.Workflows;

using RAGNET.Infrastructure.Jobs.Queue;
using RAGNET.Infrastructure.Jobs;
using RAGNET.Infrastructure.Exceptions;

using RAGNET.Application.DTOs.Workflow;
using RAGNET.Application.UseCases.WorkflowUseCases;
using RAGNET.Application.Filters;
using RAGNET.Application.Mappers;
using RAGNET.Application.Workflows.CreateWorkflow;
using RAGNET.Infrastructure.Processing;


namespace web.Controllers.WorkflowControllers
{
    [Route("api/workflows")]
    [ApiController]
    public class WorkflowController(
        IGetUserWorkflowsUseCase getUserWorkflowsUseCase,
        IDeleteWorkflowUseCase deleteWorkflowUseCase,
        IGetWorkflowUseCase getWorkflowUseCase,
        IUpdateWorkflowUseCase updateWorkflowUseCase,
        UserManager<User> userManager,
        CommandsExecutor commandsExecutor) : ControllerBase
    {
        private readonly IGetUserWorkflowsUseCase _getUserWorkflowsUseCase = getUserWorkflowsUseCase;
        private readonly IUpdateWorkflowUseCase _updateWorkflowUseCase = updateWorkflowUseCase;
        private readonly IDeleteWorkflowUseCase _deleteWorkflowUseCase = deleteWorkflowUseCase;
        private readonly IGetWorkflowUseCase _getWorkflowUseCase = getWorkflowUseCase;
        private readonly UserManager<User> _userManager = userManager;
        private readonly CommandsExecutor _commandExecutor = commandsExecutor;
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

            var workflows = await _getUserWorkflowsUseCase.Execute(user.Id);

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
                var workflowDetails = await _getWorkflowUseCase.Execute(
                    new WorkflowId(id),
                    user.Id
                );
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
                var workflow = await _updateWorkflowUseCase.Execute(
                    dto,
                    new WorkflowId(id),
                    user
                );

                return Ok(new
                {
                    workflow.Name,
                    workflow.Description,
                    workflow.IsActive,
                    ConversationProvider = workflow.ConversationProviderConfig!.ToDTOFromConversationProviderConfig(),
                    EmbeddingProvider = workflow.EmbeddingProviderConfig!.ToDTOFromEmbeddingProviderConfig()
                });
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
                var deleted = await _deleteWorkflowUseCase.Execute(
                    new WorkflowId(id),
                    user.Id
                );
                return Ok(deleted);
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
