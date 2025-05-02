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
        IUpdateCallbackUrlUseCase updateCallbackUrlUseCase,
        IDeleteCallbackUrlUseCase deleteCallbackUrlUseCase,
        UserManager<User> userManager) : ControllerBase
    {
        private readonly IAddCallbackUrlUseCase _addCallbackUrlUseCase = addCallbackUrlUseCase;
        private readonly IUpdateCallbackUrlUseCase _updateCallbackUrlUseCase = updateCallbackUrlUseCase;
        private readonly IDeleteCallbackUrlUseCase _deleteCallbackUrlUseCase = deleteCallbackUrlUseCase;
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
        public async Task<IActionResult> UpdateCallbackUrls([FromBody] CallbackUrlDTO dto, Guid callbackId, Guid workflowId)
        {
            try
            {
                var user = await _userManager.GetUserAsync(User) ?? throw new Exception();

                await _updateCallbackUrlUseCase.Execute(dto, callbackId, workflowId, user.Id);

                return Ok(new
                {
                    Message = "Callback URL Updated Successfully",
                    Url = dto
                });
            }
            catch (Exception exc)
            {
                return Problem(exc.Message);
            }
        }

        [HttpDelete("{callbackId}")]
        [Authorize]
        public async Task<IActionResult> DeleteCallbackUrls(Guid callbackId)
        {
            try
            {
                var user = await _userManager.GetUserAsync(User) ?? throw new Exception();

                var id = await _deleteCallbackUrlUseCase.Execute(callbackId, user.Id);
                return Ok(new { Message = "Deleted successfully", Id = callbackId });
            }
            catch (Exception exc)
            {
                return Problem(exc.Message);
            }
        }
    }
}