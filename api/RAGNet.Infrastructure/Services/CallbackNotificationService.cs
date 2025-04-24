using System.Net.Http.Json;
using Microsoft.Extensions.Logging;

using Polly;
using Polly.Retry;

using RAGNET.Domain.Services.Queue;


namespace RAGNET.Infrastructure.Services
{
    public class CallbackNotificationService : ICallbackNotificationService
    {
        private readonly IHttpClientFactory _httpClientFactory = null!;
        private readonly ILogger<CallbackNotificationService> _logger;
        private readonly AsyncRetryPolicy _retryPolicy;

        public CallbackNotificationService(
            IHttpClientFactory httpClientFactory,
            ILogger<CallbackNotificationService> logger)
        {
            _httpClientFactory = httpClientFactory;
            _logger = logger;

            _retryPolicy = Policy
                .Handle<HttpRequestException>()
                .WaitAndRetryAsync(new[]
                {
                    TimeSpan.FromSeconds(2),
                    TimeSpan.FromSeconds(5),
                    TimeSpan.FromSeconds(10)
                },
                onRetry: (exception, timeSpan, retryCount, context) =>
                {
                    _logger.LogWarning(
                        exception,
                        "Retry {RetryCount} after {Delay}s sending callback to {Url}",
                        retryCount,
                        timeSpan.TotalSeconds,
                        context["url"]);
                });
        }

        public async Task NotifySuccessAsync(
            Guid jobId,
            string workflowId,
            IEnumerable<string> callbackUrls,
            int processedChunks,
            int totalChunks,
            CancellationToken cancellationToken = default)
        {
            var payload = new
            {
                JobId = jobId,
                WorkflowId = workflowId,
                Status = "Completed",
                ProcessedChunks = processedChunks,
                TotalChunks = totalChunks,
                Timestamp = DateTime.UtcNow
            };

            await SendToAllAsync(callbackUrls, payload, cancellationToken);
        }

        public async Task NotifyFailureAsync(
            Guid jobId,
            string workflowId,
            IEnumerable<string> callbackUrls,
            string errorMessage,
            CancellationToken cancellationToken = default)
        {
            var payload = new
            {
                JobId = jobId,
                WorkflowId = workflowId,
                Status = "Failed",
                Error = errorMessage,
                Timestamp = DateTime.UtcNow
            };

            await SendToAllAsync(callbackUrls, payload, cancellationToken);
        }

        private async Task SendToAllAsync(
            IEnumerable<string> urls,
            object payload,
            CancellationToken cancellationToken)
        {
            var client = _httpClientFactory.CreateClient("CallbackClient");

            foreach (var url in urls)
            {
                try
                {
                    var context = new Context();
                    context["url"] = url;

                    await _retryPolicy.ExecuteAsync(
                        (context, ct) => client.PostAsJsonAsync(url, payload, ct),
                        context,
                        cancellationToken);

                    _logger.LogInformation("Callback to {Url} succeeded.", url);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Failed to send callback to {Url}.", url);
                }
            }
        }
    }

}
