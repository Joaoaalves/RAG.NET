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

        public Task NotifyProgress(Guid jobId, string userId, Domain.Entities.Document document, CancellationToken ct = default)
        {
            var group = JobStatusHub.GetGroupName(jobId.ToString());

            return _hub.Clients.Group(group)
                        .SendAsync("JobProgress", new
                        {
                            jobId,
                            userId,
                            document = new
                            {
                                document.Id,
                                document.Title,
                                Pages = document.Pages.Count
                            }
                        }, ct);
        }

        public Task NotifySuccessAsync(Guid jobId, string userId, Domain.Entities.Document document, CancellationToken ct = default)
        {
            var group = JobStatusHub.GetGroupName(jobId.ToString());
            return _hub.Clients.Group(group)
                       .SendAsync("JobCompleted", new
                       {
                           jobId,
                           userId,
                           document = new
                           {
                               document.Id,
                               document.Title,
                               Pages = document.Pages.Count
                           }
                       }, ct);
        }

        public Task NotifyFailureAsync(Guid jobId, Domain.Entities.Document document, string errorMessage, CancellationToken ct = default)
        {
            var group = JobStatusHub.GetGroupName(jobId.ToString());
            return _hub.Clients.Group(group)
                       .SendAsync("JobFailed", new
                       {
                           jobId,
                           errorMessage,
                           document = new
                           {
                               document.Id,
                               document.Title,
                               Pages = document.Pages.Count
                           }
                       }, ct);
        }
    }
}