namespace RAGNET.Domain.Services
{
    public interface IPromptService
    {
        public string GetPrompt(string category, string type);
    }
}