using Microsoft.Extensions.Configuration;

using RAGNET.Application.Providers;

namespace RAGNET.Infrastructure.Providers
{
    public class PromptService(IConfiguration configuration) : IPromptService
    {
        private readonly Dictionary<string, Dictionary<string, string>> _prompts = configuration.GetSection("Prompts").Get<Dictionary<string, Dictionary<string, string>>>()
                ?? [];

        public string GetPrompt(string category, string type)
        {
            return _prompts.TryGetValue(category, out var categoryData) &&
                   categoryData.TryGetValue(type, out var prompt)
                ? prompt
                : string.Empty;
        }
    }
}