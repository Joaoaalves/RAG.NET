using RAGNET.Domain.Enums;

namespace RAGNET.Application.DTOs.UserApiKey
{
    public class CreateUserApiKeyDTO
    {
        public string ApiKey { get; set; } = String.Empty;
        public SupportedProvider Provider { get; set; }
    }
}