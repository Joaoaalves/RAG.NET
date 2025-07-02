namespace RAGNET.Application.Subscriptions.DTOs
{
    public class CancelSubscriptionIntentDTO
    {
        public required string CustomerId { get; set; }
        public required string SubscriptionId { get; set; }
        public required DateTime CancelAt { get; set; }
    }
}