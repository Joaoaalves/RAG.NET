namespace RAGNET.Domain.Services.Queue
{
    public interface ICallbackNotificationService
    {
        public Task NotifySuccessAsync(
            Guid jobId,
            string workflowId,
            IEnumerable<string> callbackUrls,
            int processedChunks,
            int totalChunks,
            CancellationToken cancellationToken = default);

        public Task NotifyFailureAsync(
            Guid jobId,
            string workflowId,
            IEnumerable<string> callbackUrls,
            string errorMessage,
            CancellationToken cancellationToken = default);
    }
}