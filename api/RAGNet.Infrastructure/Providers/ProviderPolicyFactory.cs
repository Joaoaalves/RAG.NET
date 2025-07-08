using RAGNET.Domain.SharedKernel.Providers;
using RAGNET.Infrastructure.Providers.Policies;

namespace RAGNET.Infrastructure.Providers
{
    public class ProviderPolicyFactory : IProviderPolicyFactory
    {
        public IProviderPolicy CreatePolicy(SupportedProvider type)
        {
            return type switch
            {
                SupportedProvider.OPENAI => new OpenAiPolicy(),
                SupportedProvider.ANTHROPIC => new AnthropicPolicy(),
                SupportedProvider.VOYAGE => new VoyagePolicy(),
                SupportedProvider.GEMINI => new GeminiPolicy(),
                SupportedProvider.DEEPSEEK => new DeepSeekPolicy(),
                SupportedProvider.XAI => new XAIPolicy(),
                SupportedProvider.MISTRAL => new MistralPolicy(),
                _ => throw new ArgumentOutOfRangeException(nameof(type), "Unsupported provider")
            };
        }
    }
}
