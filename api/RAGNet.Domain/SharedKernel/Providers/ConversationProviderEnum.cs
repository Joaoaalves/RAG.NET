namespace RAGNET.Domain.SharedKernel.Providers
{
    public enum ConversationProviderEnum
    {
        OPENAI = SupportedProvider.OpenAI,
        ANTHROPIC = SupportedProvider.Anthropic,
        GEMINI = SupportedProvider.Gemini
    }
}