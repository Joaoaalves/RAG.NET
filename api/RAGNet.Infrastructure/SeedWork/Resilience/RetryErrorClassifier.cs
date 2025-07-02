namespace RAGNET.Infrastructure.SeedWork.Resilience
{
    public static class RetryErrorClassifier
    {
        public static bool IsTransientError(Exception ex)
        {
            return ex switch
            {
                HttpRequestException httpEx => IsHttpTransient(httpEx),
                TaskCanceledException => true,
                TimeoutException => true,
                _ => false
            };
        }

        private static bool IsHttpTransient(HttpRequestException ex)
        {
            var msg = ex.Message.ToLowerInvariant();

            return
                msg.Contains("429") || // Too Many Requests (rate limit)
                msg.Contains("503") || // Service Unavailable
                msg.Contains("500") || // Internal Server Error
                msg.Contains("timeout") || // Timeout in message
                msg.Contains("temporarily") || // Generic transient wording
                msg.Contains("retry"); // Sometimes error messages suggest retry
        }
    }

}