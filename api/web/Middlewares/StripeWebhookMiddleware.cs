using RAGNET.Application.Payments.Commands.ProcessSuccessfulSubscription;
using RAGNET.Application.Payments.Services;
using RAGNET.Infrastructure.Processing;

namespace web.Middlewares
{
    public class StripeWebhookMiddleware(RequestDelegate next)
    {
        private readonly RequestDelegate _next = next;

        public async Task InvokeAsync(HttpContext context, IPaymentGateway gateway, CommandsExecutor commandsExecutor)
        {
            if (context.Request.Path != "/webhooks/stripe")
            {
                await _next(context);
                return;
            }

            var json = await new StreamReader(context.Request.Body).ReadToEndAsync();
            var signature = context.Request.Headers["Stripe-Signature"];

            if (!string.IsNullOrEmpty(signature))
            {
                var userId = await gateway.ExtractUserIdFromEventAsync(json, signature!);
                if (userId is not null)
                {
                    await commandsExecutor.Execute(new ProcessSuccessfulSubscriptionCommand(userId));
                }
            }


            context.Response.StatusCode = 200;
        }
    }

}