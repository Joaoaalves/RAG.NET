namespace RAGNET.Domain.SharedKernel.Providers
{
    public enum EmbeddingProviderEnum
    {
        OPENAI = SupportedProvider.OPENAI,
        VOYAGE = SupportedProvider.VOYAGE,
        GEMINI = SupportedProvider.GEMINI,
        MISTRAL = SupportedProvider.MISTRAL
    }
}