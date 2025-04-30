using Microsoft.AspNetCore.SignalR;
using RAGNET.Domain.Services.Queue;
using RAGNET.Domain.Entities.Jobs;

namespace RAGNET.Infrastructure.Adapters.SignalR
{
    public class SignalRJobNotificationService : IJobNotificationService
    {
        private readonly IHubContext<JobStatusHub> _hub;

        public SignalRJobNotificationService(IHubContext<JobStatusHub> hub)
        {
            _hub = hub;
        }

        private Task Notify(string method, Guid jobId, string userId, Domain.Entities.Document document, Process currentProcess, CancellationToken ct = default)
        {
            var group = JobStatusHub.GetGroupName(jobId.ToString());
            return _hub.Clients.Group(group)
                           .SendAsync(method, new
                           {
                               jobId,
                               userId,
                               document = new
                               {
                                   document.Id,
                                   document.Title,
                                   Pages = document.Pages.Count
                               },
                               process = currentProcess
                           }, ct);
        }

        public Task NotifyProgress(Guid jobId, string userId, Domain.Entities.Document document, Process currentProcess, CancellationToken ct = default)
        {
            return Notify("JobProgress", jobId, userId, document, currentProcess, ct);
        }

        public Task NotifySuccessAsync(Guid jobId, string userId, Domain.Entities.Document document, CancellationToken ct = default)
        {
            return Notify("JobCompleted", jobId, userId, document, new Process
            {
                Title = "Finished",
                Progress = 100
            }, ct);
        }

        public Task NotifyFailureAsync(Guid jobId, string userId, Domain.Entities.Document document, CancellationToken ct = default)
        {

            return Notify("JobFailed", jobId, userId, document, new Process
            {
                Title = "Error",
                Progress = 100
            }, ct);
        }
    }
}