

using System.Text.Json.Serialization;
using RAGNET.Application.Converters;
using RAGNET.Domain.Enums;

namespace RAGNET.Application.DTOs.UserApiKey
{
    public class UserApiKeyDTO
    {
        public string Id { get; set; } = String.Empty;
        public string ApiKey { get; set; } = String.Empty;
        public string UserId { get; set; } = String.Empty;
        [JsonConverter(typeof(SupportedProviderConverter))]
        public SupportedProvider Provider { get; set; }
    }
}