
using RAGNET.Domain.Providers.SharedKernel.Policies;

namespace RAGNET.Domain.SharedKernel.Providers
{
    public static class ProviderPolicyFactory
    {
        public static IProviderPolicy GetPolicy(SupportedProvider type)
        {
            return type switch
            {
                SupportedProvider.OpenAI => new OpenAiPolicy(),
                SupportedProvider.Anthropic => new AnthropicPolicy(),
                SupportedProvider.Voyage => new VoyagePolicy(),
                SupportedProvider.QDrant => new QdrantPolicy(),
                SupportedProvider.Gemini => new GeminiPolicy(),
                _ => throw new ArgumentOutOfRangeException(nameof(type), "Unsupported provider")
            };
        }
    }
}
