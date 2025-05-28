using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

using RAGNET.Domain.Entities;
using RAGNET.Domain.Exceptions;

using RAGNET.Application.DTOs.Workflow;
using RAGNET.Application.UseCases.WorkflowUseCases;
using RAGNET.Application.Filters;
using RAGNET.Domain.Services.Queue;
using RAGNET.Domain.Entities.Jobs;
using RAGNET.Application.Mappers;

namespace web.Controllers.WorkflowControllers
{
    [Route("api/workflows")]
    [ApiController]
    public class WorkflowController(
        IGetUserWorkflowsUseCase getUserWorkflowsUseCase,
        ICreateWorkflowUseCase createWorkflowUseCase,
        IDeleteWorkflowUseCase deleteWorkflowUseCase,
        IGetWorkflowUseCase getWorkflowUseCase,
        IUpdateWorkflowUseCase updateWorkflowUseCase,
        UserManager<User> userManager) : ControllerBase
    {
        private readonly IGetUserWorkflowsUseCase _getUserWorkflowsUseCase = getUserWorkflowsUseCase;
        private readonly ICreateWorkflowUseCase _createWorkflowUseCase = createWorkflowUseCase;
        private readonly IUpdateWorkflowUseCase _updateWorkflowUseCase = updateWorkflowUseCase;
        private readonly IDeleteWorkflowUseCase _deleteWorkflowUseCase = deleteWorkflowUseCase;
        private readonly IGetWorkflowUseCase _getWorkflowUseCase = getWorkflowUseCase;
        private readonly UserManager<User> _userManager = userManager;

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateWorkflow([FromBody] WorkflowCreationDTO dto)
        {
            try
            {
                var user = await _userManager.GetUserAsync(User);
                if (user == null)
                    return Unauthorized();

                var workflowId = await _createWorkflowUseCase.Execute(dto, user);
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
        public async Task<IActionResult> GetWorkflow(Guid id)
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
                return Unauthorized();

            try
            {
                var workflowDetails = await _getWorkflowUseCase.Execute(id, user.Id);
                return Ok(workflowDetails);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> UpdateWorkflow([FromBody] WorkflowDetailsUpdateDTO dto, Guid id)
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
                return Unauthorized();

            try
            {
                var workflow = await _updateWorkflowUseCase.Execute(dto, id, user);

                return Ok(new
                {
                    workflow.Name,
                    workflow.Description,
                    ConversationProvider = workflow.ConversationProviderConfig.ToDTOFromConversationProviderConfig(),
                    EmbeddingProvider = workflow.EmbeddingProviderConfig.ToDTOFromEmbeddingProviderConfig()
                });
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteWorkflow(Guid id)
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
                return Unauthorized();

            try
            {
                var deleted = await _deleteWorkflowUseCase.Execute(id, user.Id);
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

                var ms = new MemoryStream();
                file.CopyTo(ms);

                var urls = workflow.CallbackUrls.Select(curl => curl.Url).ToList();
                var job = new EmbeddingJob
                {
                    ApiKey = workflow.ApiKey,
                    UserId = workflow.UserId,
                    FileName = file.FileName,
                    FileContent = ms.ToArray(),
                    CallbackUrls = urls
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
                Console.WriteLine(ex.Message);
                Response.StatusCode = 400;
                await Response.WriteAsync(ex.Message);
            }
        }
    }
}
