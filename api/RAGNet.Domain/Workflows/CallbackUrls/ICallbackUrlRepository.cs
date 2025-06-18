namespace RAGNET.Domain.Workflows.CallbackUrls
{
    public interface ICallbackUrlRepository
    {
        Task<CallbackUrl> AddAsync(CallbackUrl callbackUrl);
        Task<CallbackUrl?> GetByIdAsync(Guid id, WorkflowId workflowId);
        Task<List<CallbackUrl>> GetByWorkflowIdAsync(WorkflowId workflowId);
        Task UpdateAsync(CallbackUrl callbackUrl, WorkflowId workflowId);
        Task DeleteAsync(CallbackUrl callbackUrl, WorkflowId workflowId);
    }
}