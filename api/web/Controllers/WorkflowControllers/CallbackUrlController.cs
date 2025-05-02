using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RAGNET.Application.DTOs.CallbackUrl;
using RAGNET.Application.UseCases.CallbackUrlUseCases;
using RAGNET.Domain.Entities;

namespace web.Controllers.WorkflowControllers
{
    [Route("api/workflows/{workflowId}/callback-urls")]
    [ApiController]
    public class CallbackUrlController(
        IAddCallbackUrlUseCase addCallbackUrlUseCase,
        UserManager<User> userManager) : ControllerBase
    {
        private readonly IAddCallbackUrlUseCase _addCallbackUrlUseCase = addCallbackUrlUseCase;
        private readonly UserManager<User> _userManager = userManager;

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddCallbackUrl([FromBody] CallbackUrlDTO dto, Guid workflowId)
        {
            try
            {
                var user = await _userManager.GetUserAsync(User) ?? throw new Exception();
                var callbackUrl = await _addCallbackUrlUseCase.Execute(dto, workflowId, user.Id);

                return Ok(new { url = callbackUrl });
            }
            catch (Exception exc)
            {
                return Problem(exc.Message);
            }
        }

        [HttpPut("{callbackId}")]
        [Authorize]
        public async Task<IActionResult> UpdateCallbackUrls(Guid callbackId, Guid workflowId)
        {
            try
            {
                var user = await _userManager.GetUserAsync(User) ?? throw new Exception();
                return Ok();
            }
            catch (Exception exc)
            {
                return Problem(exc.Message);
            }
        }

        [HttpDelete("{callbackId}")]
        [Authorize]
        public async Task<IActionResult> DeleteCallbackUrls(Guid callbackId, Guid workflowId)
        {
            try
            {
                var user = await _userManager.GetUserAsync(User) ?? throw new Exception();
                return Ok(new { Message = "Deleted successfully", Id = callbackId });
            }
            catch (Exception exc)
            {
                return Problem(exc.Message);
            }
        }
    }
}