namespace RAGNET.Application.Queries.Services
{
    public interface IQueryEnhancerService
    {
        Task<List<string>> GenerateQueries(string text);
        decimal GetCostMultiplier();
    }
}