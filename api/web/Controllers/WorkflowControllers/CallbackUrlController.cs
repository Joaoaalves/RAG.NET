using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RAGNET.Application.DTOs.CallbackUrl;
using RAGNET.Domain.Entities;

namespace web.Controllers.WorkflowControllers
{
    [Route("api/workflows/{id}/callback-urls")]
    [ApiController]
    public class CallbackUrlController(UserManager<User> userManager) : ControllerBase
    {
        private readonly UserManager<User> _userManager = userManager;

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetCallbackUrls(Guid id)
        {
            try
            {
                var user = await _userManager.GetUserAsync(User) ?? throw new Exception();
                List<string> urls = [];
                return Ok(new { urls });
            }
            catch (Exception exc)
            {
                return Problem(exc.Message);
            }
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddCallbackUrl([FromBody] CallbackUrlDTo dto, Guid id)
        {
            try
            {
                var user = await _userManager.GetUserAsync(User) ?? throw new Exception();
                return Ok(new { url = dto });
            }
            catch (Exception exc)
            {
                return Problem(exc.Message);
            }
        }

        [HttpPut("{callbackId}")]
        [Authorize]
        public async Task<IActionResult> UpdateCallbackUrls(Guid callbackId, Guid id)
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
        public async Task<IActionResult> DeleteCallbackUrls(Guid callbackId, Guid id)
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