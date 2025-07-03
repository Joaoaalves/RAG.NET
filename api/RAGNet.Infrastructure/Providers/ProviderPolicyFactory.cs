using RAGNET.Domain.SharedKernel.Providers;
using RAGNET.Infrastructure.Providers.Policies;

namespace RAGNET.Infrastructure.Providers
{
    public class ProviderPolicyFactory : IProviderPolicyFactory
    {
        public IProviderPolicy GetPolicy(SupportedProvider type)
        {
            return type switch
            {
                SupportedProvider.OpenAI => new OpenAiPolicy(),
                SupportedProvider.Anthropic => new AnthropicPolicy(),
                SupportedProvider.Voyage => new VoyagePolicy(),
                SupportedProvider.Qdrant => new QdrantPolicy(),
                SupportedProvider.Gemini => new GeminiPolicy(),
                SupportedProvider.Deepseek => new DeepSeekPolicy(),
                SupportedProvider.XAI => new XAIPolicy(),
                SupportedProvider.Mistral => new MistralPolicy(),
                _ => throw new ArgumentOutOfRangeException(nameof(type), "Unsupported provider")
            };
        }
    }
}
