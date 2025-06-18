namespace RAGNET.Application.UserQueries
{
    public interface IQueryEnhancerService
    {
        Task<List<string>> GenerateQueries(string text);
    }
}