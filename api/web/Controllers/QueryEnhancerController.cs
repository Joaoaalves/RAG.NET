using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

// Domain
using RAGNET.Domain.Entities;

// Application
using RAGNET.Application.DTOs.QueryEnhancer;
using RAGNET.Application.Mappers;
using RAGNET.Application.UseCases.QueryEnhancerUseCases;
using RAGNET.Application.UseCases.WorkflowUseCases;
using RAGNET.Domain.Enums;

namespace web.Controllers
{
    [Route("api/workflows")]
    [ApiController]
    public class QueryEnhancerController(ICreateQueryEnhancerUseCase createQueryEnhancerUseCase, IGetWorkflowUseCase getWorkflowUseCase, UserManager<User> userManager) : ControllerBase
    {
        readonly ICreateQueryEnhancerUseCase _createQueryEnhancerUseCase = createQueryEnhancerUseCase;
        readonly IGetWorkflowUseCase _getWorkflowUseCase = getWorkflowUseCase;
        readonly UserManager<User> _userManager = userManager;

        [HttpPost("{workflowId}/query-enhancer/auto-query")]
        [Authorize]
        public async Task<IActionResult> EnableAutoQuery([FromBody] AutoQueryCreationDTO dto, Guid workflowId)
        {
            try
            {
                var user = await _userManager.GetUserAsync(User);
                if (user == null)
                    return Unauthorized();

                var workflow = await _getWorkflowUseCase.Execute(workflowId, user.Id);

                if (workflow == null)
                    return Unauthorized();

                if (workflow.QueryEnhancers.Any(qe => qe.Type == QueryEnhancerStrategy.AUTO_QUERY))
                    return BadRequest("Auto Query already enabled!");


                var qeCreationDTO = dto.ToQueryEnhancerDTOFromAutoQueryCreationDTO();

                var queryEnhancer = await _createQueryEnhancerUseCase.Execute(qeCreationDTO, workflow.Id);

                return Ok(new { Message = "Auto Query enabled!", QueryEnhancerId = queryEnhancer.Id });
            }
            catch (Exception exc)
            {
                return Problem(exc.Message);
            }
        }

        [HttpPost("{workflowId}/query-enhancer/hyde")]
        [Authorize]
        public async Task<IActionResult> EnableHyde([FromBody] HyDECreationDTO dto, Guid workflowId)
        {
            try
            {
                var user = await _userManager.GetUserAsync(User);
                if (user == null)
                    return Unauthorized();

                var workflow = await _getWorkflowUseCase.Execute(workflowId, user.Id);

                if (workflow == null)
                    return Unauthorized();

                if (workflow.QueryEnhancers.Any(qe => qe.Type == QueryEnhancerStrategy.HYPOTHETICAL_DOCUMENT_EMBEDDING))
                    return BadRequest("HyDE already enabled!");

                var qeCreationDTO = dto.ToQueryEnhancerDTOFromHyDECreationDTO();

                var queryEnhancer = await _createQueryEnhancerUseCase.Execute(qeCreationDTO, workflow.Id);

                return Ok(new { Message = "Hyde enabled!", QueryEnhancerId = queryEnhancer.Id });
            }
            catch (Exception exc)
            {
                return Problem(exc.Message);
            }
        }

    }
}