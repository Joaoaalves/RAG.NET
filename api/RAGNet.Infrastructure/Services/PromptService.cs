using Microsoft.Extensions.Configuration;
using RAGNET.Domain.Services;

namespace RAGNET.Infrastructure.Services
{
    public class PromptService : IPromptService
    {
        private readonly Dictionary<string, Dictionary<string, string>> _prompts;

        public PromptService(IConfiguration configuration)
        {
            _prompts = configuration.GetSection("Prompts").Get<Dictionary<string, Dictionary<string, string>>>()
                ?? [];
        }

        public string GetPrompt(string category, string type)
        {
            return _prompts.TryGetValue(category, out var categoryData) &&
                   categoryData.TryGetValue(type, out var prompt)
                ? prompt
                : string.Empty;
        }
    }
}