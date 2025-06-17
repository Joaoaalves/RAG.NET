namespace RAGNET.Application.Query
{
    public interface IQueryEnhancerService
    {
        Task<List<string>> GenerateQueries(string text);
    }
}