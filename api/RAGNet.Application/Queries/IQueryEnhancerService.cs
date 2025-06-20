namespace RAGNET.Application.Queries
{
    public interface IQueryEnhancerService
    {
        Task<List<string>> GenerateQueries(string text);
    }
}