using DeepSeek.Core;
using DeepSeek.Core.Models;

namespace RAGNET.Infrastructure.ChatCompletions.DeepSeek
{
    public class DeepSeekClientWrapper(
        string apiKey,
        string model
    ) : IDeepSeekClientWrapper
    {
        private readonly DeepSeekClient _client = new(apiKey);
        private readonly string _model = model;
        public async Task<string> CompleteChatAsync(List<Message> messages, ResponseFormat? responseFormat = null, CancellationToken ct = default)
        {
            var request = new ChatRequest
            {
                Messages = messages,
                Model = _model,
                ResponseFormat = responseFormat
            };

            var chatResponse = await _client.ChatAsync(request, ct);
            if (chatResponse is not null)
            {
                var firstChoice = chatResponse.Choices.FirstOrDefault();

                if (firstChoice?.Message?.Content is null)
                    throw new Exception("Internal error");
                return firstChoice.Message.Content;
            }

            throw new HttpRequestException(_client.ErrorMsg);
        }
    }
}