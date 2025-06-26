using RAGNET.Application.Payments.DTOs;
using RAGNET.Application.Payments.Services;
using StackExchange.Redis;

namespace RAGNET.Infrastructure.Redis
{
    public class RedisPaymentStatusRepository(IConnectionMultiplexer redis) : IPaymentStatusService
    {
        private readonly IConnectionMultiplexer _redis = redis;

        private static string GetStatusKey(string paymentIntentId, string userId) =>
            $"{paymentIntentId}:{userId}";

        public async Task<bool> CreatePaymentIntent(PaymentIntentDTO intent)
        {
            try
            {
                var db = _redis.GetDatabase();
                var key = GetStatusKey(intent.PaymentIntentId, intent.UserId);
                await db.StringSetAsync(
                    key,
                    PaymentStatus.PENDING.ToString(),
                    TimeSpan.FromMinutes(30) // TTL 30 Minutes
                );
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Redis] Error on CreatePaymentIntent: {ex.Message}");
                return false;
            }
        }

        public async Task<PaymentStatus?> GetPaymentIntent(PaymentIntentDTO intent)
        {
            try
            {
                var db = _redis.GetDatabase();
                var key = GetStatusKey(intent.PaymentIntentId, intent.UserId);
                var value = await db.StringGetAsync(key);

                if (value.HasValue &&
                    Enum.TryParse<PaymentStatus>(value, ignoreCase: true, out var status))
                {
                    return status;
                }

                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Redis] Error on GetPaymentIntent: {ex.Message}");
                return null;
            }
        }

        public async Task<PaymentStatus?> UpdatePaymentStatus(PaymentIntentDTO intent, PaymentStatus status)
        {
            try
            {
                var db = _redis.GetDatabase();
                var key = GetStatusKey(intent.PaymentIntentId, intent.UserId);

                var exists = await db.KeyExistsAsync(key);
                if (!exists)
                    return null;

                await db.StringSetAsync(key, status.ToString());
                return status;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Redis] Error on UpdatePaymentStatus: {ex.Message}");
                return null;
            }
        }

        public async Task<PaymentStatus?> RemovePaymentIntent(PaymentIntentDTO intent)
        {
            try
            {
                var db = _redis.GetDatabase();
                var key = GetStatusKey(intent.PaymentIntentId, intent.UserId);

                var currentStatus = await GetPaymentIntent(intent);
                if (currentStatus == null)
                    return null;

                await db.KeyDeleteAsync(key);
                return currentStatus;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Redis] Error on RemovePaymentIntent: {ex.Message}");
                return null;
            }
        }

    }
}
