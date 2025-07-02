using RAGNET.Infrastructure.SeedWork.Resilience;

namespace tests.RAGNet.Infrastructure.Tests.Resilience
{
    public class RetryHelperTests
    {
        [Fact]
        public async Task RetryHelper_ShouldRetryOnTransientError()
        {
            int attempts = 0;

            string result = await RetryHelper.ExecuteWithRetryAsync(async () =>
            {
                attempts++;
                await Task.Delay(1);
                if (attempts < 3)
                    throw new HttpRequestException("503 Service Unavailable");

                return "Success";
            }, baseDelayMs: 1);

            Assert.Equal("Success", result);
            Assert.Equal(3, attempts);
        }

        [Fact]
        public async Task RetryHelper_ShouldThrowAfterMaxRetries()
        {
            int attempts = 0;

            await Assert.ThrowsAsync<HttpRequestException>(async () =>
            {
                await RetryHelper.ExecuteWithRetryAsync<string>(async () =>
                {
                    attempts++;
                    await Task.Delay(1);
                    throw new HttpRequestException("429 Too Many Requests");
                }, baseDelayMs: 1, maxRetries: 2);
            });

            Assert.Equal(3, attempts); // 1 try + 2 retries
        }

        [Fact]
        public async Task RetryHelper_ShouldNotRetryIfErrorIsNotTransient()
        {
            int attempts = 0;

            await Assert.ThrowsAsync<InvalidOperationException>(async () =>
            {
                await RetryHelper.ExecuteWithRetryAsync<bool>(async () =>
                {
                    attempts++;
                    await Task.Delay(1);
                    throw new InvalidOperationException("Critical failure");
                }, baseDelayMs: 1);
            });

            Assert.Equal(1, attempts);
        }
    }
}