using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using RAGNET.Domain.SeedWork;

namespace RAGNET.Infrastructure.SeedWork
{
    public class TypedIdValueConverter<TTypedIdValue>(ConverterMappingHints? mappingHints = null) : ValueConverter<TTypedIdValue, Guid>(
            id => id.Value,
            value => Create(value),
            mappingHints)
        where TTypedIdValue : TypedIdValueBase
    {
        private static TTypedIdValue Create(Guid id)
        {
            return (Activator.CreateInstance(typeof(TTypedIdValue), id) as TTypedIdValue)!;
        }
    }
}