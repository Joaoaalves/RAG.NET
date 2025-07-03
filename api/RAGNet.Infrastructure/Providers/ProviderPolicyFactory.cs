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
                SupportedProvider.QDrant => new QdrantPolicy(),
                SupportedProvider.Gemini => new GeminiPolicy(),
                SupportedProvider.DeepSeek => new DeepSeekPolicy(),
                _ => throw new ArgumentOutOfRangeException(nameof(type), "Unsupported provider")
            };
        }
    }
}
