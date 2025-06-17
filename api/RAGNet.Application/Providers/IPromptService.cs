namespace RAGNET.Application.Providers
{
    public interface IPromptService
    {
        public string GetPrompt(string category, string type);
    }
}