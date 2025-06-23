namespace RAGNET.Application.Infrastructure.Providers
{
    public interface IPromptService
    {
        public string GetPrompt(string category, string type);
    }
}