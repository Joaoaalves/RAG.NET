using StackExchange.Redis;
using System.Text.Json;

using RAGNET.Domain.Entities.Jobs;

using RAGNET.Domain.Services.Queue;

namespace RAGNET.Infrastructure.Adapters.Queue
{
    public class RedisJobQueueService(IConnectionMultiplexer redis) : IJobQueueService
    {
        private readonly IDatabase _db = redis.GetDatabase();
        private const string QueueKey = "embedding_jobs_queue";

        public async Task EnqueueAsync(EmbeddingJob job)
        {
            string json = JsonSerializer.Serialize(job);
            await _db.ListRightPushAsync(QueueKey, json);
        }

        public async Task<List<EmbeddingJob>> GetPendingJobsAsync()
        {
            var length = await _db.ListLengthAsync(QueueKey);
            var values = await _db.ListRangeAsync(QueueKey, 0, length - 1);
            return values.Select(v => JsonSerializer.Deserialize<EmbeddingJob>(v!)!).ToList();
        }

        public async Task<EmbeddingJob?> DequeueAsync()
        {
            var value = await _db.ListLeftPopAsync(QueueKey);
            return value.HasValue ? JsonSerializer.Deserialize<EmbeddingJob>(value!) : null;
        }
    }
}