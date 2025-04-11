namespace RAGNET.Domain.Services.Query
{
    public interface IQueryEnhancerService
    {
        Task<List<string>> GenerateQueries(string text);
    }
}