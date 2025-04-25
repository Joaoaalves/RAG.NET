using System.Net.Http.Json;
using Microsoft.Extensions.Logging;

using Polly;
using Polly.Retry;
using RAGNET.Domain.Entities;
using RAGNET.Domain.Entities.Jobs;
using RAGNET.Domain.Enums;
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
                .WaitAndRetryAsync(
                [
                    TimeSpan.FromSeconds(2),
                    TimeSpan.FromSeconds(5),
                    TimeSpan.FromSeconds(10)
                ],
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

        private static IEnumerable<string> GetUrls(List<CallbackUrl> callbacks)
        {
            return callbacks.Select(c => c.Url);
        }

        public async Task NotifySuccessAsync(
            Job job,
            int processedChunks,
            CancellationToken cancellationToken = default)
        {
            var payload = new
            {
                job.JobId,
                job.WorkflowId,
                Status = JobStatus.DONE,
                ProcessedChunks = processedChunks,
                Timestamp = DateTime.UtcNow
            };

            await SendToAllAsync(
                GetUrls(job.CallbackUrls),
                payload,
                cancellationToken
            );
        }

        public async Task NotifyFailureAsync(
            Job job,
            string errorMessage,
            CancellationToken cancellationToken = default)
        {
            var payload = new
            {
                job.JobId,
                job.WorkflowId,
                Status = JobStatus.FAILED,
                Error = errorMessage,
                Timestamp = DateTime.UtcNow
            };

            await SendToAllAsync(
                GetUrls(job.CallbackUrls),
                payload,
                cancellationToken
            );
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
