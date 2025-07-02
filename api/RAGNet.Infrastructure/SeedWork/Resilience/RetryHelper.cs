namespace RAGNET.Infrastructure.SeedWork.Resilience
{
    public static class RetryHelper
    {
        private static readonly Random _jitter = new();

        public static async Task<T> ExecuteWithRetryAsync<T>(
            Func<Task<T>> operation,
            int maxRetries = 5,
            int baseDelayMs = 2000,
            Func<Exception, bool>? shouldRetry = null)
        {
            int attempt = 0;

            while (true)
            {
                try
                {
                    return await operation();
                }
                catch (Exception ex) when ((shouldRetry ?? RetryErrorClassifier.IsTransientError)(ex))
                {
                    attempt++;
                    if (attempt > maxRetries) throw;

                    int delay = (int)(Math.Pow(2, attempt) * baseDelayMs);
                    delay += _jitter.Next(0, 1000);

                    await Task.Delay(delay);
                }
            }
        }
    }
}