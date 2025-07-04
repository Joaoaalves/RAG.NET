using Microsoft.AspNetCore.SignalR;
using RAGNET.Domain.Documents;
using RAGNET.Infrastructure.Jobs;
using RAGNET.Infrastructure.Jobs.Queue;

namespace RAGNET.Infrastructure.SignalR
{
    public class SignalRJobNotificationService(IHubContext<JobStatusHub> hub) : IJobNotificationService
    {
        private readonly IHubContext<JobStatusHub> _hub = hub;

        private Task Notify(string method, Guid jobId, string userId, Document document, ProcessDTO currentProcess, CancellationToken ct = default)
        {
            var group = JobStatusHub.GetGroupName(userId);

            return _hub.Clients.Group(group)
                           .SendAsync(method, new
                           {
                               jobId,
                               userId,
                               document = new
                               {
                                   document.Id,
                                   Title = document.Title.Value,
                                   Pages = document.Pages.Count
                               },
                               process = currentProcess
                           }, ct);
        }

        public Task NotifyProgress(Guid jobId, string userId, Document document, ProcessDTO currentProcess, CancellationToken ct = default)
        {
            return Notify("JobProgress", jobId, userId, document, currentProcess, ct);
        }

        public Task NotifySuccessAsync(Guid jobId, string userId, Document document, CancellationToken ct = default)
        {
            return Notify("JobCompleted", jobId, userId, document, new ProcessDTO
            {
                Title = "Finished",
                Progress = 100
            }, ct);
        }

        public Task NotifyFailureAsync(Guid jobId, string userId, Document document, string errorMessage, CancellationToken ct = default)
        {

            return Notify("JobFailed", jobId, userId, document, new ProcessDTO
            {
                Title = errorMessage,
                Progress = 100
            }, ct);
        }
    }
}