using RAGNET.Domain.Entities;

namespace RAGNET.Domain.Services
{
    public interface IQueryEnhancerService
    {
        Task<List<string>> GenerateQueries(string text);
    }
}