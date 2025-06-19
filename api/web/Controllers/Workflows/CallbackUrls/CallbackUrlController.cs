using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

using RAGNET.Application.Workflows.CallbackUrls.CreateCallbackUrl;
using RAGNET.Application.Workflows.CallbackUrls.DeleteCallbackUrl;
using RAGNET.Application.Workflows.CallbackUrls.UpdateCallbackUrl;
using RAGNET.Application.Workflows.CreateWorkflow;
using RAGNET.Domain.SharedKernel.URLs;
using RAGNET.Domain.Users;
using RAGNET.Domain.Workflows;
using RAGNET.Domain.Workflows.CallbackUrls;
using RAGNET.Infrastructure.Processing;

namespace web.Controllers.Workflows.CallbackUrls
{
    [Route("api/workflows/{workflowId}/callback-urls")]
    [ApiController]
    public class CallbackUrlController(
        CommandsExecutor commandsExecutor,
        UserManager<User> userManager) : ControllerBase
    {
        private readonly CommandsExecutor _commandsExecutor = commandsExecutor; private readonly UserManager<User> _userManager = userManager;

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddCallbackUrl([FromBody] CreateCallbackUrlRequest request, [FromRoute] Guid workflowId)
        {
            try
            {
                var user = await _userManager.GetUserAsync(User) ?? throw new Exception();
                var command = new CreateCallbackUrlCommand(
                    new WorkflowId(workflowId),
                    user.Id,
                    request.Url
                );

                var callbackUrl = await _commandsExecutor.Execute(
                    command
                );

                return Ok(new { url = callbackUrl });
            }
            catch (Exception exc)
            {
                return Problem(exc.Message);
            }
        }

        [HttpPut("{callbackId}")]
        [Authorize]
        public async Task<IActionResult> UpdateCallbackUrls([FromBody] UpdateCallbackUrlRequest request, Guid callbackId, [FromRoute] Guid workflowId)
        {
            try
            {
                var user = await _userManager.GetUserAsync(User) ?? throw new Exception();

                var command = new UpdateCallbackUrlCommand(
                    new WorkflowId(workflowId),
                    user.Id,
                    new CallbackUrlId(callbackId),
                    URL.Create(request.Url)
                );

                var dto = await _commandsExecutor.Execute(command);

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
        public async Task<IActionResult> DeleteCallbackUrls(Guid callbackId, [FromRoute] Guid workflowId)
        {
            try
            {
                var user = await _userManager.GetUserAsync(User) ?? throw new Exception();

                var command = new DeleteCallbackUrlCommand(
                    new WorkflowId(callbackId),
                    user.Id,
                    new CallbackUrlId(callbackId)
                );

                await _commandsExecutor.Execute(command);

                return Ok(new { Message = "Deleted successfully", Id = callbackId });
            }
            catch (Exception exc)
            {
                return Problem(exc.Message);
            }
        }
    }
}