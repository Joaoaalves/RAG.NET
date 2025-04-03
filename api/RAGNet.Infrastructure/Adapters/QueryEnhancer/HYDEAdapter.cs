using RAGNET.Domain.Services;

namespace RAGNET.Infrastructure.Adapters.QueryEnhancer
{
    public class HYDEAdapter(int maxQueries, IChatCompletionService completionService) : IQueryEnhancerService
    {
        private readonly int _maxQueries = maxQueries;
        private readonly IChatCompletionService _completionService = completionService;
        public Task<List<string>> GenerateQueries(string text)
        {
            throw new NotImplementedException();
        }
    }
}