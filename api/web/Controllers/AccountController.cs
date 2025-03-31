using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RAGNET.Application.DTOs.Account;
using RAGNET.Application.Mappers;
using RAGNET.Domain.Entities;

namespace web.Controllers
{
    [ApiController]
    [Route("/api/")]
    public class AccountController(UserManager<User> userManager) : ControllerBase
    {
        private readonly UserManager<User> _userManager = userManager;

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDTO model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = new User
            {
                UserName = model.Email,
                Email = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName
            };

            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                    ModelState.AddModelError(string.Empty, error.Description);
                return BadRequest(ModelState);
            }

            return Ok(new { Message = "User registered successfully" });
        }
        [HttpGet("info")]
        [Authorize]
        public async Task<IActionResult> GetUserInfo()
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
                return Unauthorized();

            return Ok(user.ToAccountInfoDTOFromUser());
        }
    }

}