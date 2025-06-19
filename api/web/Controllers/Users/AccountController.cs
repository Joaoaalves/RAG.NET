using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RAGNET.Application.DTOs.Account;
using RAGNET.Application.UseCases.Account;

namespace web.Controllers.Users
{
    [ApiController]
    [Route("/api/")]
    public class AccountController(
    IRegisterUser registerUserUseCase,
    IGetUserInfo getUserInfoUseCase) : ControllerBase
    {
        private readonly IRegisterUser _register = registerUserUseCase;
        private readonly IGetUserInfo _getUserInfo = getUserInfoUseCase;

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDTO model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var (success, errors) = await _register.ExecuteAsync(model);

            if (!success)
            {
                foreach (var error in errors)
                    ModelState.AddModelError(string.Empty, error);
                return BadRequest(ModelState);
            }

            return Ok(new { Message = "User registered successfully" });
        }

        [HttpGet("info")]
        [Authorize]
        public async Task<IActionResult> GetUserInfo()
        {
            var dto = await _getUserInfo.ExecuteAsync(User);
            return dto is null ? Unauthorized() : Ok(dto);
        }
    }

}

