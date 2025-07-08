namespace RAGNET.Domain.SharedKernel.Providers
{
    public enum ConversationProviderEnum
    {
        OPENAI = SupportedProvider.OPENAI,
        ANTHROPIC = SupportedProvider.ANTHROPIC,
        GEMINI = SupportedProvider.GEMINI,
        DeepSeek = SupportedProvider.DEEPSEEK,
        XAI = SupportedProvider.XAI,
        Mistral = SupportedProvider.MISTRAL
    }
}