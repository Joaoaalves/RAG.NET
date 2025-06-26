using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RAGNET.Application.Payments.Commands.CreateCheckout;
using RAGNET.Application.Payments.DTOs;
using RAGNET.Infrastructure.Processing;

namespace web.Controllers.Subscriptions
{
    [Route("api/checkout")]
    [ApiController]
    public class SubscriptionsController(
        CommandsExecutor commandsExecutor
    ) : ControllerBase
    {
        private readonly CommandsExecutor _commandsExecutor = commandsExecutor;

        [HttpPost("start")]
        [Authorize]
        public async Task<IActionResult> StartCheckout([FromBody] StartCheckoutDTO dto)
        {
            try
            {
                var command = new CreateCheckoutCommand(dto.SuccessUrl, dto.CancelUrl);
                var url = await _commandsExecutor.Execute(command);

                return Ok(new { url });
            }
            catch (Exception exc)
            {
                return Problem(exc.Message);
            }
        }
    }
}