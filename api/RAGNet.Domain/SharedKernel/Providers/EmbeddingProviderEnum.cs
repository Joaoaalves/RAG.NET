namespace RAGNET.Domain.SharedKernel.Providers
{
    public enum EmbeddingProviderEnum
    {
        OPENAI = SupportedProvider.OpenAI,
        VOYAGE = SupportedProvider.Voyage,
        GEMINI = SupportedProvider.Gemini
    }
}