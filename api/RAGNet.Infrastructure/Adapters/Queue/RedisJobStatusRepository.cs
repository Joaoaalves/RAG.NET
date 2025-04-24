using StackExchange.Redis;

using RAGNET.Domain.Repositories;
using RAGNET.Domain.Enums;

namespace RAGNET.Infrastructure.Adapters.Queue
{
    public class RedisJobStatusRepository(IConnectionMultiplexer redis) : IJobStatusRepository
    {
        private readonly IConnectionMultiplexer _redis = redis;

        public Task SetPendingAsync(Guid jobId) =>
            _redis.GetDatabase().StringSetAsync($"job:{jobId}", JobStatus.PENDING.ToString());

        public Task MarkAsCompletedAsync(Guid jobId) =>
            _redis.GetDatabase().StringSetAsync($"job:{jobId}", JobStatus.DONE.ToString());

        public async Task<JobStatus?> TryGetStatusAsync(Guid jobId)
        {
            var status = await _redis.GetDatabase().StringGetAsync($"job:{jobId}");

            if (status.HasValue)
            {
                if (Enum.TryParse<JobStatus>(status, ignoreCase: true, out var result))
                {
                    return result;
                }
            }

            return null;
        }
    }
}