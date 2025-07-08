using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using RAGNET.Domain.Users.ApiKeys;

namespace RAGNET.Infrastructure.SeedWork
{
    public sealed class ApiKeyConverter(ConverterMappingHints? mappingHints = null)
    : ValueConverter<ApiKey, string>(
        apiKey => apiKey.Value,
        value => new ApiKey(value),
        mappingHints
    )
    { }
}