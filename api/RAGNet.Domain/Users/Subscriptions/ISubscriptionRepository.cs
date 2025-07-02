namespace RAGNET.Domain.Users.Subscriptions
{
    public interface ISubscriptionRepository
    {
        Task<Subscription?> GetByUserIdAsync(string userId);
        Task UpdateAsync(Subscription subscription);
    }
}