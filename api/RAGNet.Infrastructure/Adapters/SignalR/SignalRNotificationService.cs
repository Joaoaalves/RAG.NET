using Microsoft.AspNetCore.SignalR;
using RAGNET.Domain.Services.Queue;

namespace RAGNET.Infrastructure.Adapters.SignalR
{
    public class SignalRJobNotificationService : IJobNotificationService
    {
        private readonly IHubContext<JobStatusHub> _hub;

        public SignalRJobNotificationService(IHubContext<JobStatusHub> hub)
        {
            _hub = hub;
        }

        public Task NotifySuccessAsync(Guid jobId, string userId, CancellationToken ct = default)
        {
            var group = JobStatusHub.GetGroupName(jobId.ToString());
            return _hub.Clients.Group(group)
                       .SendAsync("JobCompleted", new { jobId, userId }, ct);
        }

        public Task NotifyFailureAsync(Guid jobId, string userId, string errorMessage, CancellationToken ct = default)
        {
            var group = JobStatusHub.GetGroupName(jobId.ToString());
            return _hub.Clients.Group(group)
                       .SendAsync("JobFailed", new { jobId, errorMessage }, ct);
        }
    }
}