namespace RAGNET.Domain.Workflows
{
    public interface IWorkflowRepository
    {
        Task<Workflow?> GetByIdAsync(Guid id, string? userId);
        Task<IEnumerable<Workflow>> GetAllAsync(string? userId);
        Task<Workflow> AddAsync(Workflow workflow);
        Task UpdateAsync(Workflow workflow, string? userId);
        Task DeleteAsync(Workflow workflow, string? userId);
        Task<Workflow?> GetByApiKey(string apiKey);
        Task<IEnumerable<Workflow>> GetUserWorkflows(string userId);
        Task UpdateByApiKey(Workflow workflow, string apiKey);
    }
}