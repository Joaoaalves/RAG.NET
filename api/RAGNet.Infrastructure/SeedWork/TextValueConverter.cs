using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using RAGNET.Domain.Documents.Pages;

namespace RAGNET.Infrastructure.SeedWork
{
    public sealed class TextValueConverter(ConverterMappingHints? mappingHints = null) : ValueConverter<Text, string>(
              text => text.Value,
              value => new Text(value),
              mappingHints)
    {
    }
}