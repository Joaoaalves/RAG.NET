using Microsoft.AspNetCore.SignalR;

namespace RAGNET.Infrastructure.Adapters.SignalR
{
    public class JobStatusHub : Hub
    {
        public override Task OnConnectedAsync()
        {
            return base.OnConnectedAsync();
        }

        public Task JoinJobGroup(string jobId)
        {
            var groupName = GetGroupName(jobId);
            return Groups.AddToGroupAsync(Context.ConnectionId, groupName);
        }

        public Task LeaveJobGroup(string jobId)
        {
            var groupName = GetGroupName(jobId);
            return Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);
        }

        public static string GetGroupName(string jobId) => $"job:{jobId}";
    }
}