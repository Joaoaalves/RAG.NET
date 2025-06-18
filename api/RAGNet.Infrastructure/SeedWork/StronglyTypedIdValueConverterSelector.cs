using System.Collections.Concurrent;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using RAGNET.Domain.SeedWork;

namespace RAGNET.Infrastructure.SeedWork
{
    public class StronglyTypedIdValueConverterSelector(ValueConverterSelectorDependencies dependencies) : ValueConverterSelector(dependencies)
    {
        private readonly ConcurrentDictionary<(Type ModelClrType, Type ProviderClrType), ValueConverterInfo> _converters
            = new();

        public override IEnumerable<ValueConverterInfo> Select(Type modelClrType, Type? providerClrType = null)
        {
            foreach (var converter in base.Select(modelClrType, providerClrType))
            {
                yield return converter;
            }

            var underlyingModelType = UnwrapNullableType(modelClrType);
            var underlyingProviderType = UnwrapNullableType(providerClrType);

            if (underlyingProviderType is null || underlyingProviderType == typeof(Guid))
            {
                if (typeof(TypedIdValueBase).IsAssignableFrom(underlyingModelType))
                {
                    var converterType = typeof(TypedIdValueConverter<>).MakeGenericType(underlyingModelType);

                    yield return _converters.GetOrAdd(
                        (underlyingModelType, typeof(Guid)),
                        _ => new ValueConverterInfo(
                            modelClrType: modelClrType,
                            providerClrType: typeof(Guid),
                            factory: valueConverterInfo =>
                                (ValueConverter)Activator.CreateInstance(converterType, valueConverterInfo.MappingHints)! // "!" para garantir não nulo
                        )
                    );
                }
            }
        }

        private static Type? UnwrapNullableType(Type? type)
        {
            return type;
        }
    }
}