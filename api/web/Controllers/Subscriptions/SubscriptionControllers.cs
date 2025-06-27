using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RAGNET.Application.Subscriptions.Commands.CreateCheckout;
using RAGNET.Application.Subscriptions.DTOs;
using RAGNET.Application.Subscriptions.Queries.GetPaymentStatus;
using RAGNET.Application.Subscriptions.Services;
using RAGNET.Infrastructure.Processing;

namespace web.Controllers.Subscriptions
{
    [Route("api/checkout")]
    [ApiController]
    public class SubscriptionsController(
        CommandsExecutor commandsExecutor,
        QueriesExecutor queriesExecutor
    ) : ControllerBase
    {
        private readonly CommandsExecutor _commandsExecutor = commandsExecutor;
        private readonly QueriesExecutor _queriesExecutor = queriesExecutor;

        [HttpPost("start")]
        [Authorize]
        public async Task<IActionResult> StartCheckout([FromBody] StartCheckoutDTO dto)
        {
            try
            {
                var command = new CreateSubscriptionCheckoutCommand(dto.PlanType, dto.SuccessUrl, dto.CancelUrl);
                var url = await _commandsExecutor.Execute(command);

                return Ok(new { url });
            }
            catch (Exception exc)
            {
                return Problem(exc.Message);
            }
        }

        [HttpGet("status")]
        [Authorize]
        public async Task<IActionResult> CheckPaymentStatus([FromQuery] string paymentId)
        {
            try
            {
                var query = new GetPaymentStatusQuery(paymentId);

                var status = await _queriesExecutor.Execute(query);

                if (status is not null)
                {
                    return Ok(new
                    {
                        status
                    });
                }

                return NotFound(new { message = "Payment not found" });
            }
            catch (Exception exc)
            {
                return Problem(exc.Message);
            }
        }
    }
}