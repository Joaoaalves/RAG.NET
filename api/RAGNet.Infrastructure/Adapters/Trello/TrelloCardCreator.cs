using System.Web;
using Microsoft.Extensions.Configuration;
using RAGNET.Domain.Entities;
using RAGNET.Domain.Services;

namespace RAGNET.Infrastructure.Adapters.Trello
{
    public class TrelloCardCreator(HttpClient http, IConfiguration config) : ICardCreatorService
    {
        private readonly HttpClient _http = http;
        private readonly string _apiKey = config["Trello:ApiKey"] ?? throw new ArgumentNullException("Trello:ApiKey");
        private readonly string _token = config["Trello:Token"] ?? throw new ArgumentNullException("Trello:Token");
        private readonly string _listId = config["Trello:ListId"] ?? throw new ArgumentNullException("Trello:ListId");

        public async Task CreateCardAsync(Card card)
        {
            var builder = new UriBuilder("https://api.trello.com/1/cards");
            var query = HttpUtility.ParseQueryString(string.Empty);
            query["key"] = _apiKey;
            query["token"] = _token;
            query["idList"] = _listId;
            query["name"] = card.Name;
            query["desc"] = card.Description;
            builder.Query = query.ToString();

            var response = await _http.PostAsync(builder.Uri, null);
            response.EnsureSuccessStatusCode();
        }
    }
}