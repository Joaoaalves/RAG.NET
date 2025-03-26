using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using RAGNET.Application.DTOs.Workflow;
using RAGNET.Domain.Entities;
using RAGNET.Application.UseCases.WorkflowUseCases;
using RAGNET.Application.UseCases.EmbeddingUseCases;
using RAGNET.Application.Attributes;

namespace web.Controllers
{
    [Route("api/workflows")]
    [ApiController]
    public class WorkflowController(
        IGetUserWorkflowsUseCase getUserWorkflowsUseCase,
        ICreateWorkflowUseCase createWorkflowUseCase,
        IGetWorkflowUseCase getWorkflowUseCase,
        IProcessEmbeddingUseCase processEmbeddingUseCase,
        UserManager<User> userManager) : ControllerBase
    {
        private readonly IGetUserWorkflowsUseCase _getUserWorkflowsUseCase = getUserWorkflowsUseCase;
        private readonly ICreateWorkflowUseCase _createWorkflowUseCase = createWorkflowUseCase;
        private readonly IGetWorkflowUseCase _getWorkflowUseCase = getWorkflowUseCase;
        private readonly IProcessEmbeddingUseCase _processEmbeddingUseCase = processEmbeddingUseCase;
        private readonly UserManager<User> _userManager = userManager;

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateWorkflow([FromBody] WorkflowCreationDTO dto)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return Unauthorized();

            var workflowId = await _createWorkflowUseCase.Execute(dto, user);
            return Ok(new { Message = "Workflow criado com sucesso!", WorkflowId = workflowId });
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

        [HttpPost("embedding/{workflow_id}")]
        [Consumes("multipart/form-data")]
        [ApiKeyCheck]
        public async Task<IActionResult> ProcessEmbedding(Guid workflow_id, IFormFile file)
        {
            try
            {
                var apiKey = Request.Headers["x-api-key"].ToString();
                int processedChunks = await _processEmbeddingUseCase.Execute(file, apiKey);
                return Ok(new { Message = "Embedding done successfully.", ProcessedChunks = processedChunks });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
