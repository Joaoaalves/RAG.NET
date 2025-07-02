using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using RAGNET.Domain.SharedKernel.Tokens;

namespace RAGNET.Infrastructure.SeedWork
{
    public sealed class TokenAmountConverter(ConverterMappingHints? mappingHints = null)
        : ValueConverter<TokenAmount, long>(
            amount => amount.Value,
            value => TokenAmount.FromMilitokens(value),
            mappingHints)
    { }
}
