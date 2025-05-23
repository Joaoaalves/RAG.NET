using RAGNET.Application.DTOs.UserApiKey;
using RAGNET.Domain.Entities;

namespace RAGNET.Application.Mappers
{
    public static class UserApiKeyMapper
    {
        public static UserApiKey ToUserApiKey(this CreateUserApiKeyDTO dto, string userId, string suffix)
        {
            return new UserApiKey
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                KeySuffix = suffix,
                ApiKey = dto.ApiKey,
                Provider = dto.Provider
            };
        }

        public static UserApiKeyDTO ToDTO(this UserApiKey userApiKey)
        {
            return new UserApiKeyDTO
            {
                Id = userApiKey.Id.ToString(),
                ApiKey = userApiKey.KeySuffix,
                UserId = userApiKey.UserId,
                Provider = userApiKey.Provider
            };
        }
    }
}