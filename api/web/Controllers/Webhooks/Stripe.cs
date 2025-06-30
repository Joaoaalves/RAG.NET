using Microsoft.AspNetCore.Mvc;
using RAGNET.Application.Subscriptions.Commands.ProcessSuccessfulCancelSubscription;
using RAGNET.Application.Subscriptions.Commands.ProcessSuccessfulSubscription;
using RAGNET.Application.Subscriptions.Services;
using RAGNET.Infrastructure.Processing;

namespace web.Controllers.Webhooks
{
    [ApiController]
    [Route("api/webhooks/stripe")]
    public class StripeWebhookController(
        IPaymentGateway gateway,
        CommandsExecutor commandsExecutor
    ) : ControllerBase
    {
        private readonly IPaymentGateway _gateway = gateway;
        private readonly CommandsExecutor _commandsExecutor = commandsExecutor;

        [HttpPost]
        public async Task<IActionResult> HandleWebhook()
        {
            using var reader = new StreamReader(Request.Body);
            var json = await reader.ReadToEndAsync();

            var signature = Request.Headers["Stripe-Signature"];

            if (!string.IsNullOrEmpty(signature))
            {
                var intent = await _gateway.ExtractPaymentIntentFromEventAsync(json, signature!);
                if (intent is not null)
                {
                    await _commandsExecutor.Execute(new ProcessSuccessfulSubscriptionCommand(intent));
                }

                var cancelIntent = await _gateway.ExtractCancelIntentFromEventAsync(json, signature!);
                if (cancelIntent is not null)
                {
                    await _commandsExecutor.Execute(new ProcessSuccessfulCancelSubscriptionCommand(cancelIntent));
                }
            }

            return Ok();
        }
    }
}
