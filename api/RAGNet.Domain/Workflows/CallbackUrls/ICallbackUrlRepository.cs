namespace RAGNET.Domain.Workflows.CallbackUrls
{
    public interface ICallbackUrlRepository
    {
        Task<CallbackUrl> AddAsync(CallbackUrl callbackUrl);
        Task<CallbackUrl?> GetByIdAsync(Guid id, Guid workflowId);
        Task<List<CallbackUrl>> GetByWorkflowIdAsync(Guid workflowId);
        Task UpdateAsync(CallbackUrl callbackUrl, Guid workflowId);
        Task DeleteAsync(CallbackUrl callbackUrl, Guid workflowId);
    }
}