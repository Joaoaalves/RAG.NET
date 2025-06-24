using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using RAGNET.Application.Users.Queries.GetUserDetails;
using RAGNET.Application.Users.Commands.RegisterUser;

using RAGNET.Infrastructure.Processing;

namespace web.Controllers.Users
{
    [ApiController]
    [Route("/api/")]
    public class AccountController(
        QueriesExecutor queriesExecutor,
        CommandsExecutor commandsExecutor
    ) : ControllerBase
    {
        private readonly QueriesExecutor _queriesExecutor = queriesExecutor;
        private readonly CommandsExecutor _commandsExecutor = commandsExecutor;
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterUserRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var command = new RegisterUserCommand(request.FirstName, request.LastName, request.Email, request.Password);

            var (userId, errors) = await _commandsExecutor.Execute(command);

            if (string.IsNullOrEmpty(userId))
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
            var query = new GetUserDetailsQuery(User);

            var dto = await _queriesExecutor.Execute(query);

            return dto is null ? Unauthorized() : Ok(dto);
        }
    }

}

